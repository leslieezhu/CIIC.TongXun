﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;
/// <summary>
/// https://www.aspsnippets.com/Articles/Using-Windows-Forms-WebBrowser-control-in-ASPNet.aspx
/// System.Windows.Forms    System.Windows.Forms.DataVisualization 
/// </summary>
namespace CIIC.TongXun
{
    public class HtmlCapture
    {
        private WebBrowser web;
        private Timer tready;
        private Rectangle screen;
        private Size? imgsize = null;

        //an event that triggers when the html document is captured
        public delegate void HtmlCaptureEvent(object sender, Uri url, Bitmap image);
        public event HtmlCaptureEvent HtmlImageCapture;

        //class constructor
        public HtmlCapture()
        {
            //initialise the webbrowser and the timer
            web = new WebBrowser();
            tready = new Timer();
            tready.Interval = 2000;
            screen = Screen.PrimaryScreen.Bounds;
            //set the webbrowser width and hight
            web.Width = screen.Width;
            web.Height = screen.Height;
            //suppress script errors and hide scroll bars
            web.ScriptErrorsSuppressed = true;
            web.ScrollBarsEnabled = false;
            //attached events
            web.Navigating +=
              new WebBrowserNavigatingEventHandler(web_Navigating);
            web.DocumentCompleted += new
              WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);
            tready.Tick += new EventHandler(tready_Tick);
        }

        #region Public methods
        public void Create(string url)
        {
            imgsize = null;
            web.Navigate(url);
        }

        public void Create(string url, Size imgsz)
        {
            this.imgsize = imgsz;
            web.Navigate(url);
        }
        #endregion

        #region Events
        void web_DocumentCompleted(object sender,
                 WebBrowserDocumentCompletedEventArgs e)
        {
            //start the timer
            tready.Start();
        }

        void web_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //stop the timer   
            tready.Stop();
        }

        void tready_Tick(object sender, EventArgs e)
        {
            //stop the timer
            tready.Stop();
            //get the size of the document's body
            Rectangle body = web.Document.Body.ScrollRectangle;

            //check if the document width/height is greater than screen width/height
            Rectangle docRectangle = new Rectangle()
            {
                Location = new Point(0, 0),
                Size = new Size(body.Width > screen.Width ? body.Width : screen.Width,
                 body.Height > screen.Height ? body.Height : screen.Height)
            };
            //set the width and height of the WebBrowser object
            web.Width = docRectangle.Width;
            web.Height = docRectangle.Height;

            //if the imgsize is null, the size of the image will 
            //be the same as the size of webbrowser object
            //otherwise  set the image size to imgsize
            Rectangle imgRectangle;
            if (imgsize == null)
                imgRectangle = docRectangle;
            else
                imgRectangle = new Rectangle()
                {
                    Location = new Point(0, 0),
                    Size = imgsize.Value
                };
            //create a bitmap object 
            Bitmap bitmap = new Bitmap(imgRectangle.Width, imgRectangle.Height);
            //get the viewobject of the WebBrowser
            IViewObject ivo = web.Document.DomDocument as IViewObject;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //get the handle to the device context and draw
                IntPtr hdc = g.GetHdc();
                ivo.Draw(1, -1, IntPtr.Zero, IntPtr.Zero,
                         IntPtr.Zero, hdc, ref imgRectangle,
                         ref docRectangle, IntPtr.Zero, 0);
                g.ReleaseHdc(hdc);
            }
            //invoke the HtmlImageCapture event
            HtmlImageCapture(this, web.Url, bitmap);

        }
        #endregion
    }
}