namespace Freshoot
{
    partial class OrdersOrganizer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdersOrganizer));
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.total_order_count_label = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.available_truck_count_label = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.listOrder = new System.Windows.Forms.ListBox();
            this.pictureTruckNumber = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.truck_bins_count = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ordercntcontrol = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.size_label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.driver_name_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTruck = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listTruck = new System.Windows.Forms.CheckedListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.OrderUp = new System.Windows.Forms.PictureBox();
            this.OrderDown = new System.Windows.Forms.PictureBox();
            this.OrderRefresh = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruckNumber)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordercntcontrol)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::Freshoot.Properties.Resources.exit_button;
            this.pictureBox6.Location = new System.Drawing.Point(915, 12);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(50, 50);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 9;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.OnExit);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Freshoot.Properties.Resources.load_locations;
            this.pictureBox5.Location = new System.Drawing.Point(778, 12);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(50, 50);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 8;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.OnLoadGeoInformation);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Freshoot.Properties.Resources.load_order_icon;
            this.pictureBox3.Location = new System.Drawing.Point(848, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(50, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.OnLoadOrder);
            // 
            // total_order_count_label
            // 
            this.total_order_count_label.AutoSize = true;
            this.total_order_count_label.Location = new System.Drawing.Point(352, 48);
            this.total_order_count_label.Name = "total_order_count_label";
            this.total_order_count_label.Size = new System.Drawing.Size(25, 13);
            this.total_order_count_label.TabIndex = 5;
            this.total_order_count_label.Text = "150";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(416, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Available Trucks";
            // 
            // available_truck_count_label
            // 
            this.available_truck_count_label.AutoSize = true;
            this.available_truck_count_label.Location = new System.Drawing.Point(520, 48);
            this.available_truck_count_label.Name = "available_truck_count_label";
            this.available_truck_count_label.Size = new System.Drawing.Size(13, 13);
            this.available_truck_count_label.TabIndex = 3;
            this.available_truck_count_label.Text = "5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(281, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Total Orders";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(33, 42);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label12.Location = new System.Drawing.Point(29, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(286, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "Order Organization 7th. June 2019";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(433, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Truck Orders";
            // 
            // listOrder
            // 
            this.listOrder.FormattingEnabled = true;
            this.listOrder.Location = new System.Drawing.Point(433, 39);
            this.listOrder.Name = "listOrder";
            this.listOrder.Size = new System.Drawing.Size(544, 95);
            this.listOrder.TabIndex = 16;
            this.listOrder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListOrder_MouseClick);
            // 
            // pictureTruckNumber
            // 
            this.pictureTruckNumber.Location = new System.Drawing.Point(326, 82);
            this.pictureTruckNumber.Name = "pictureTruckNumber";
            this.pictureTruckNumber.Size = new System.Drawing.Size(50, 50);
            this.pictureTruckNumber.TabIndex = 15;
            this.pictureTruckNumber.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(220, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Number Figure";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(338, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "134";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Week Deliver";
            // 
            // truck_bins_count
            // 
            this.truck_bins_count.AutoSize = true;
            this.truck_bins_count.Location = new System.Drawing.Point(337, 26);
            this.truck_bins_count.Name = "truck_bins_count";
            this.truck_bins_count.Size = new System.Drawing.Size(25, 13);
            this.truck_bins_count.TabIndex = 11;
            this.truck_bins_count.Text = "120";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel3.Controls.Add(this.OrderRefresh);
            this.panel3.Controls.Add(this.OrderDown);
            this.panel3.Controls.Add(this.OrderUp);
            this.panel3.Controls.Add(this.ordercntcontrol);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.listOrder);
            this.panel3.Controls.Add(this.pictureTruckNumber);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.truck_bins_count);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.size_label);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.driver_name_label);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.comboTruck);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(275, 531);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(989, 150);
            this.panel3.TabIndex = 5;
            // 
            // ordercntcontrol
            // 
            this.ordercntcontrol.Location = new System.Drawing.Point(259, 23);
            this.ordercntcontrol.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ordercntcontrol.Name = "ordercntcontrol";
            this.ordercntcontrol.Size = new System.Drawing.Size(41, 20);
            this.ordercntcontrol.TabIndex = 18;
            this.ordercntcontrol.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ordercntcontrol.ValueChanged += new System.EventHandler(this.OnChangeOrderCnt);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "bins";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Orders";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "AHD-67889";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Number";
            // 
            // size_label
            // 
            this.size_label.AutoSize = true;
            this.size_label.Location = new System.Drawing.Point(80, 83);
            this.size_label.Name = "size_label";
            this.size_label.Size = new System.Drawing.Size(103, 13);
            this.size_label.TabIndex = 5;
            this.size_label.Text = "40 bins accomodate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Size";
            // 
            // driver_name_label
            // 
            this.driver_name_label.AutoSize = true;
            this.driver_name_label.Location = new System.Drawing.Point(80, 52);
            this.driver_name_label.Name = "driver_name_label";
            this.driver_name_label.Size = new System.Drawing.Size(66, 13);
            this.driver_name_label.TabIndex = 3;
            this.driver_name_label.Text = "David Arthur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Driver";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Truck";
            // 
            // comboTruck
            // 
            this.comboTruck.FormattingEnabled = true;
            this.comboTruck.Location = new System.Drawing.Point(80, 18);
            this.comboTruck.Name = "comboTruck";
            this.comboTruck.Size = new System.Drawing.Size(121, 21);
            this.comboTruck.TabIndex = 0;
            this.comboTruck.SelectedIndexChanged += new System.EventHandler(this.ComboTruck_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel2.Controls.Add(this.pictureBox6);
            this.panel2.Controls.Add(this.pictureBox5);
            this.panel2.Controls.Add(this.pictureBox3);
            this.panel2.Controls.Add(this.total_order_count_label);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.available_truck_count_label);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(275, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(989, 74);
            this.panel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.listTruck);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 681);
            this.panel1.TabIndex = 3;
            // 
            // listTruck
            // 
            this.listTruck.CheckOnClick = true;
            this.listTruck.FormattingEnabled = true;
            this.listTruck.Location = new System.Drawing.Point(15, 147);
            this.listTruck.Name = "listTruck";
            this.listTruck.Size = new System.Drawing.Size(237, 379);
            this.listTruck.TabIndex = 4;
            this.listTruck.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListTruck_ItemCheck);
            this.listTruck.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListTruck_MouseClick);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label14.Location = new System.Drawing.Point(12, 109);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(126, 20);
            this.label14.TabIndex = 3;
            this.label14.Text = "Trucks on Map";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(275, 74);
            this.panel4.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Freshoot.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(275, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemmory = 5;
            this.gmap.Location = new System.Drawing.Point(281, 80);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 25;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(971, 445);
            this.gmap.TabIndex = 6;
            this.gmap.Zoom = 11D;
            this.gmap.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.Gmap_OnMarkerClick);
            // 
            // OrderUp
            // 
            this.OrderUp.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.OrderUp.Location = new System.Drawing.Point(522, 14);
            this.OrderUp.Name = "OrderUp";
            this.OrderUp.Size = new System.Drawing.Size(46, 18);
            this.OrderUp.TabIndex = 19;
            this.OrderUp.TabStop = false;
            this.OrderUp.Click += new System.EventHandler(this.OrderUp_Click);
            // 
            // OrderDown
            // 
            this.OrderDown.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.OrderDown.Location = new System.Drawing.Point(592, 14);
            this.OrderDown.Name = "OrderDown";
            this.OrderDown.Size = new System.Drawing.Size(46, 18);
            this.OrderDown.TabIndex = 20;
            this.OrderDown.TabStop = false;
            this.OrderDown.Click += new System.EventHandler(this.OrderDown_Click);
            // 
            // OrderRefresh
            // 
            this.OrderRefresh.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.OrderRefresh.Location = new System.Drawing.Point(662, 15);
            this.OrderRefresh.Name = "OrderRefresh";
            this.OrderRefresh.Size = new System.Drawing.Size(46, 18);
            this.OrderRefresh.TabIndex = 21;
            this.OrderRefresh.TabStop = false;
            this.OrderRefresh.Click += new System.EventHandler(this.OrderRefresh_Click);
            // 
            // OrdersOrganizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.gmap);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OrdersOrganizer";
            this.Text = "Orders Organizer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruckNumber)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordercntcontrol)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrderRefresh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label total_order_count_label;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label available_truck_count_label;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox listOrder;
        private System.Windows.Forms.PictureBox pictureTruckNumber;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label truck_bins_count;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label size_label;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label driver_name_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTruck;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.CheckedListBox listTruck;
        private System.Windows.Forms.NumericUpDown ordercntcontrol;
        private System.Windows.Forms.PictureBox OrderRefresh;
        private System.Windows.Forms.PictureBox OrderDown;
        private System.Windows.Forms.PictureBox OrderUp;
    }
}