﻿using System;
using System.Windows.Forms;

namespace ObjectDetecting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for that application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
