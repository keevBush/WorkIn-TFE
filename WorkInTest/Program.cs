using FirebaseNet.Messaging;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WorkInTest
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            try
            {
                FCMClient client = new FCMClient("AAAACZAcRsE:APA91bH0culzhfx1sCo5pybkZIwywXzVfYVkQ9-TWx4nVV1D5teJY-TLA9N570Wn4Rmb7GY98Jh6DBpcaru9vw7sA_7SsuGh4Jf4-2ES3_3YfsRBqOyFcGCWygB0I6C5wewlPVsGiv4T");

                var message = new Message()
                {
                    To = "eSEBDKMsprk:APA91bFbrkfzAN13Oh0LhA7CHWECWuruw2GqROqMNxDrjqLII0wWRDbGKChrKuJ3hxB-NpBpDJu-_FjQipZeDuH5L10sx2rxpYr7hG2U3jMbtTdFIzn4i3-xMuU6Y9aAkfzPe4m2Btnt",
                    Notification = new AndroidNotification()
                    {
                        Body = "great match!",
                        Title = "Portugal vss Denmark"
                    }
                };
                
                var result = await client.SendMessageAsync(message);
                Console.WriteLine(result);
                Console.ReadKey();
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
    
}
