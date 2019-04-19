using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Linq;
using Android;
using Android.Content.PM;

namespace bluetooth_test
{
    [Activity(Label = "bluetooth test", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        BluetoothConnection myConnection = new BluetoothConnection();


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Button buttonConnect = FindViewById<Button>(Resource.Id.button1);
            Button buttonDisconnect = FindViewById<Button>(Resource.Id.button2);

            Button button1On = FindViewById<Button>(Resource.Id.button3);
            Button button2On = FindViewById<Button>(Resource.Id.button4);

            TextView connected = FindViewById<TextView>(Resource.Id.textView1);
            BluetoothSocket _socket = null;

            System.Threading.Thread listenThread = new System.Threading.Thread(listener);
            listenThread.Abort();

            buttonConnect.Click += delegate
            {
                listenThread.Start();

                myConnection = new BluetoothConnection();

                myConnection.getAdapter();

                myConnection.thisAdapter.StartDiscovery();

                try
                {
                    myConnection.getDevice();

                    myConnection.thisDevice.SetPairingConfirmation(false);

                    myConnection.thisDevice.SetPairingConfirmation(true);

                    myConnection.thisDevice.CreateBond();
                }
                catch (Exception deviceEX)
                {
                }
                myConnection.thisAdapter.CancelDiscovery();

                _socket = myConnection.thisDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                myConnection.thisSocket = _socket;

                try
                {

                    myConnection.thisSocket.Connect();

                    connected.Text = "Connected!";

                    buttonDisconnect.Enabled = true;

                    buttonConnect.Enabled = false;

                    if (listenThread.IsAlive == false)
                    {
                        listenThread.Start();
                    }
                }
                catch (Exception CloseEX)
                {

                }
            };

            buttonDisconnect.Click += delegate
            {
                try
                {
                    buttonConnect.Enabled = true;
                    listenThread.Abort();

                    myConnection.thisDevice.Dispose();

                    myConnection.thisSocket.OutputStream.WriteByte(187);
                    myConnection.thisSocket.OutputStream.Close();

                    myConnection.thisSocket.Close();

                    myConnection = new BluetoothConnection();
                    _socket = null;

                    connected.Text = "Disconnected!";
                }
                catch { }
            };




            button1On.Click += delegate
            {
                try
                {
                    myConnection.thisSocket.OutputStream.WriteByte(1);
                    myConnection.thisSocket.OutputStream.WriteByte(1);
                    myConnection.thisSocket.OutputStream.WriteByte(1);
                    myConnection.thisSocket.OutputStream.Close();
                }
                catch (Exception outPutEX)
                {

                }
            };

            button2On.Click += delegate
            {
                try
                {
                    myConnection.thisSocket.OutputStream.WriteByte(2);
                    myConnection.thisSocket.OutputStream.WriteByte(2);
                    myConnection.thisSocket.OutputStream.WriteByte(2);
                    myConnection.thisSocket.OutputStream.Close();
                }
                catch (Exception outPutEX)
                {

                }


            };

            void listener()
            {
                byte[] read = new byte[1];

                TextView readTextView = FindViewById<TextView>(Resource.Id.textView2);
                TextView timeTextView = FindViewById<TextView>(Resource.Id.textView3);
                while (true)
                {
                    try
                    {

                        myConnection.thisSocket.InputStream.Read(read, 0, 1);
                        myConnection.thisSocket.InputStream.Close();
                        RunOnUiThread(() =>
                        {

                            if (read[0] == 1)
                            {

                                readTextView.Text = "Relais AN";
                            }
                            else if (read[0] == 0)
                            {
                                readTextView.Text = "Relais AUS";
                                timeTextView.Text = "";
                            }
                        });
                    }
                    catch { }

                }
            }







        }


        public class BluetoothConnection
        {

            public void getAdapter() { this.thisAdapter = BluetoothAdapter.DefaultAdapter; }
            public void getDevice() { this.thisDevice = (from bd in this.thisAdapter.BondedDevices where bd.Name == "HC-06" select bd).FirstOrDefault(); }

            public BluetoothAdapter thisAdapter { get; set; }
            public BluetoothDevice thisDevice { get; set; }

            public BluetoothSocket thisSocket { get; set; }



        }
    }
}

