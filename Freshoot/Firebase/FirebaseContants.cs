using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FireSharp.Config;
using Firebase.Auth;
using FireSharp.Interfaces;

namespace Freshoot.Firebase
{
    class FirebaseContants
    {
        public static IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "I4pJsdus8wbOP63wUsInPCLny8u0FhP1ih8Pa3H2",
            BasePath = "https://freshoot-8d88a.firebaseio.com/"
        };

        public static FirebaseAuthOptions fAuthOptions = new FirebaseAuthOptions("AIzaSyBpFAlPU2Oeemom1HVhL7B5h3-5EGtSwFg"); //Web API key
    }
}
