/*
 * Copyright (c) 2013 Tomasz Hachaj
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using Series3D2;
using System.Windows.Forms;

namespace WindowsGame2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Form1 f = new Form1();
            f.ShowDialog();
            int port = f.port;
            String ip = f.ip;
            float fow = f.fieldOfView;
            Microsoft.Xna.Framework.Vector4 color = 
                new Microsoft.Xna.Framework.Vector4((float)((double)f.color.R / 255.0),
                    (float)((double)f.color.G / 255.0),
                    (float)((double)f.color.B / 255.0),
                    1.0f);
            if (ip == "none")
                return;
            try
            {
                using (Game1 game = new Game1(ip, port, color, f.uid, fow, f.shipModel, f.randomPosition))
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Source +
                    "\r\n-------------------------------------\r\n" +
                    e.Message +
                    "\r\n-------------------------------------\r\n" +
                    e.StackTrace, "Deadly Sky Massacre - Deadly Exception");
            }
        }
    }
#endif
}

