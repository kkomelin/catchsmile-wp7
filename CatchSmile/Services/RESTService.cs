using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using CatchSmile.Models;
using Microsoft.Phone.Net.NetworkInformation;
using System.Xml.Linq;
using System.Linq;


namespace CatchSmile.Services
{
    public class RESTService
    {
        private string serviceUri;

        public RESTService(string serviceUri)
        {
            this.serviceUri = serviceUri;
        }

        public void GetNode(int nid, Action<Node> onFinish = null, Action<Exception> onError = null) 
        {
            // Check network availability.
            if(!DeviceNetworkInformation.IsNetworkAvailable) 
            {
                onError(new Exception("Network is unavailable!"));
            }

            WebClient webClient = new WebClient();

            String requestString = String.Format("{0}node/{1}.xml", this.serviceUri, nid);

            webClient.OpenReadCompleted += delegate (object sender, OpenReadCompletedEventArgs e) 
            {
                if(e.Error != null)
                {
                    onError(e.Error);
                }

                XElement resultXml = XElement.Load(e.Result);
                XElement xEl = resultXml.Element("title");

                Node node = new Node();
                node.Title = xEl.Value;

                onFinish(node);
            };

            // Call the OpenReadAsyc to make a GET request.
            webClient.OpenReadAsync(new Uri(requestString));
        }
    }
}
