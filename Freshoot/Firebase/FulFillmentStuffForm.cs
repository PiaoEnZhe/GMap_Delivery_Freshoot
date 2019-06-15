using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Freshoot.Firebase
{
    public partial class FulFillmentStuffForm : Form
    {
        private Order[] data = Order.getSampleData();
        private string barcode_buffer = null;
        private System.Threading.Timer barcodeTimer = null;
        private bool timerblock = false;

        public FulFillmentStuffForm()
        {
            InitializeComponent();
        }

        private void FulFillmentStuffForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            reloadOrdersData(data);
            updateOrderHeader(data[0]);
            updateProductList(data[0]);

        }

        private void reloadOrdersData(Order[] orders) {
            order_list.Items.Clear();
            for (int i = 0; i < data.Length; i++) {
                var item = new ListViewItem(new string[] { orders[i].order_code, orders[i].addressnumber + " " + orders[i].streetname,
                    orders[i].getProducts().Length.ToString(), orders[i].bins.Length.ToString(), orders[i].getCheckedCount().ToString(),
                    orders[i].getRemainingCount().ToString(), (orders[i].getRemainingCount() == 0).ToString() });
                if (orders[i].getRemainingCount() == 0) {
                    item.BackColor = Color.Green;
                }
                order_list.Items.Add(item);
            }
        }

        private void updateOrderHeader(Order selected) {
            order_detail_bin_id.Text = selected.order_code;
            order_detail_address.Text = selected.addressnumber + " " + selected.streetname;
            order_detail_product_count.Text = selected.getProducts().Length.ToString();
            order_detail_bins_count.Text = selected.bins.Length.ToString();
            order_detail_bins_checked.Text = selected.getCheckedCount().ToString();
            order_detail_bins_remaining.Text = selected.getRemainingCount().ToString();
            order_detail_description.Text = selected.note;

            resultPicture.Image = selected.getRemainingCount() == 0 ? Freshoot.Properties.Resources.checker_sucess : Freshoot.Properties.Resources.checker_failure;
        }

        private void updateProductList(Order selected) {
            product_list.Items.Clear();
            var products = selected.getProducts();
            for (int i = 0; i < products.Length; i++) {
                var substitute = products[i].status == Freshoot.Product.STATUS_SEC_REPLACE ? products[i].secondary_barcode : "X";
                var lowQ = products[i].status == Freshoot.Product.STATUS_LOW_QUALITY ? "O" : "X";
                product_list.Items.Add(new ListViewItem(new string[] {products[i].barcode, products[i].title, substitute, lowQ, products[i].getPrice().ToString(), "0.1", ""}));
            }
        }

        private void updateBinDetails(OrderBin binChoice)
        {
            barcode_read_label.Text = binChoice.barcode;
            BarcodeUtils.showBarcode(binChoice.barcode, barcodePreview);

            bin_detail_bin_num.Text = binChoice.binNo.ToString();
            bin_detail_lane_num.Text = binChoice.laneNo.ToString();
            bin_detail_order_id.Text = binChoice.orderId.ToString();
            bin_detail_product_count.Text = binChoice.products.Length.ToString();
        }

        private void FulFillmentStuffForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (barcode_buffer != null && barcode_buffer.Length > 4) {
                    onBarcodeInput(barcode_buffer);
                }
            }
            else {
                if (barcode_buffer == null) {
                    barcode_buffer = "";
                }
                barcode_buffer += (char)e.KeyCode;
                postBarcodeTimer();
            }
        }

        private void postBarcodeTimer() {
            Trace.WriteLine((barcodeTimer == null).ToString() + " timerblock: " + timerblock.ToString());
            /*if (barcodeTimer != null) {
                timerblock = true;
                barcodeTimer.Dispose();
            }*/

            barcodeTimer = new System.Threading.Timer(async obj =>
            {
                barcode_buffer = null;
                /*if (timerblock) {
                    timerblock = false;
                    barcodeTimer = null;
                } else {

                    barcode_buffer = null;
                    barcodeTimer = null;
                }*/
            }, null, 300, System.Threading.Timeout.Infinite);
        }

        private void onBarcodeInput(string barcode) {
            var orderChoice = Order.getOrder(data, barcode);
            var binChoice = Order.getOrderBin(data, barcode);

            if (orderChoice != null && binChoice != null) {
                binChoice.isChecked = true;
                updateOrderHeader(orderChoice);
                updateProductList(orderChoice);
                updateBinDetails(binChoice);
            }
            reloadOrdersData(data);
        }

    }
}
