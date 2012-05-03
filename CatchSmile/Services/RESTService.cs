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
using Microsoft.Phone.Net.NetworkInformation;
using System.Xml.Linq;
using System.Linq;
using CatchSmile.Model;
using System.IO;


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
                try
                {
                    if (e.Error != null)
                    {
                        if (onError != null)
                        {
                            onError(e.Error);
                        }
                    }

                    XElement resultXml = XElement.Load(e.Result);
                    XElement xEl = resultXml.Element("title");

                    Node node = new Node();
                    node.Title = xEl.Value;

                    if (onFinish != null)
                    {
                        onFinish(node);
                    }
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        onError(ex);
                    }
                }
            };

            // Call the OpenReadAsyc to make a GET request.
            webClient.OpenReadAsync(new Uri(requestString));
        }

        public void CreateNode(Node node, Action<Node> onFinish = null, Action<Exception> onError = null)
        {
            // Check network availability.
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                onError(new Exception("Network is unavailable!"));
            }

            WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            webClient.Headers[HttpRequestHeader.Accept] = "application/xml";

            String requestString = String.Format("{0}node", this.serviceUri);

            String requestQuery = String.Format("title={0}&type={1}&catchsmile_image[fid]={2}&catchsmile_image[list]=1&catchsmile_image[data]=", node.Title, node.Type, node.File.Fid);

            webClient.UploadStringCompleted += delegate(object sender, UploadStringCompletedEventArgs e)
            {
                try
                {
                    if (e.Error != null)
                    {
                        if (onError != null)
                        {
                            onError(e.Error);
                        }
                    }
                   
                    /* Response example:
                       <?xml version="1.0" encoding="utf-8"?>
                       <result>
                          <nid>26</nid>
                          <uri>http://drupal7/endpoint1/node/26</uri>
                       </result>
                     */

                    XElement resultXml = XElement.Parse(e.Result);

                    node.Nid = int.Parse(resultXml.Element("nid").Value);
                    node.Uri = resultXml.Element("uri").Value;

                    if (onFinish != null)
                    {
                        onFinish(node);
                    }
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        onError(ex);
                    }
                }
            };

            // Call the OpenWriteAsyc to make a POST request.
            webClient.UploadStringAsync(new Uri(requestString), "POST", requestQuery);
        }

        /// <summary>
        /// Creates a file in Drupal remotelly.
        /// 
        /// To allow creating files through Services module you need to set 'Save file information' Drupal permission.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="onFinish"></param>
        /// <param name="onError"></param>
        public void CreateFile(CatchSmile.Model.File file, Action<CatchSmile.Model.File> onFinish = null, Action<Exception> onError = null)
        {
            // Check network availability.
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                onError(new Exception("Network is unavailable!"));
            }

            WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            webClient.Headers[HttpRequestHeader.Accept] = "application/xml";

            String requestString = String.Format("{0}file", this.serviceUri);

            // TODO: url-encoding, building url parameters from a collection.

            String requestQuery = String.Format("filesize={0}&filename={1}&file={2}&uid={3}", file.FileSize, file.FileName, file.FileContent, file.Uid);

            webClient.UploadStringCompleted += delegate(object sender, UploadStringCompletedEventArgs e)
            {
                try
                {
                    if (e.Error != null)
                    {
                        if (onError != null)
                        {
                            onError(e.Error);
                        }
                    }

                    /* Response example:
                       <?xml version="1.0" encoding="utf-8"?>
                       <result>
                        <fid>17</fid>
                        <uri>http://drupal7/endpoint1/file/17</uri>
                       </result>
                     */

                    XElement resultXml = XElement.Parse(e.Result);

                    file.Fid = int.Parse(resultXml.Element("fid").Value);
                    file.Uri = resultXml.Element("uri").Value;

                    if (onFinish != null)
                    {
                        onFinish(file);
                    }
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        onError(ex);
                    }
                }
            };

            // Call the OpenWriteAsyc to make a POST request.
            webClient.UploadStringAsync(new Uri(requestString), "POST", requestQuery);
        }
    }
}
