// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Author: Utkarsh Mathur
// Email: u7karsh@yahoo.co.in
// Website: u7karsh.com
// Date: 23 March 2015
// Code: ArduinoTTS (C# part)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using SpeechLib;

namespace VectorApp
{
    class Program
    {
        //serial object
        SerialPort serialPort;
        Program(String Port, int baud)
        {
            serialPort = new SerialPort(Port, baud, Parity.None, 8);
            serialPort.Open();
            int wakeup = 3;
            //a little delay allowing hardware to respond. just for safety (not mandatory)
            for (int i = 0; i < wakeup; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
        }
        String getData()
        {
            String data = "";
            //header polling for data integrity
            while (true)
            {
                char header = (Char)serialPort.ReadByte();
                //poll untill head $ is found
                if (header == '$')
                {
                    header = (Char)serialPort.ReadByte(); // this is for >
                    break;
                }
            }
            //number of bytes in data
            int raw_datalength = serialPort.ReadByte(); //bytes of data
            //computing crc checksum for error analysis.
            int checksum = 0;
            checksum = (checksum ^ raw_datalength);

            //reading data payload
            int[] raw_data = new int[raw_datalength];
            for (int i = 0; i < raw_datalength; i++)
            {
                raw_data[i] = serialPort.ReadByte();
                checksum = checksum ^ raw_data[i]; //XOR
            }

            int raw_checksum = serialPort.ReadByte();
            //check mismatch checksum
            if (checksum != raw_checksum)
            {
                Console.WriteLine("Data Corrupted!!");
            }
            //data accepted
            else
            {
                //constructing string..
                for (int i = 0; i < raw_datalength; i++)
                {
                    data = data + (char)raw_data[i];
                }
            }
            return data;
        }
        static void Main()
        {
            //speech object
            SpVoice Voice = new SpVoice();
            Voice.Voice = Voice.GetVoices("gender=female").Item(0);

            //chenge this accordingly
            String port = "COM14";
            //instantiating serial object.
            Console.WriteLine("Attempting serial connection at port " + port + "..");
            Program p = new Program(port, 115200);
            //a little delay ensuring all instructions are completed and nothing is left in the pipeline
            int sleep = 10;
            Console.WriteLine("Sleeping for " + sleep + " seconds..");
            for (int i = 0; i < sleep; i++)
            {
                Console.WriteLine(sleep - i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("Connection established at port " + port + "..");

            //main loop
            while (true)
            {
                //Polling data
                String textCmd = p.getData();
                Console.WriteLine( textCmd );
                Voice.Speak(textCmd, SpeechVoiceSpeakFlags.SVSFDefault);
            }
        }
    }
}
