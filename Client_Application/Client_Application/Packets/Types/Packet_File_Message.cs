using Client_Application.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Application.Packets.Types
{
    internal class Packet_File_Message : Packet_Base
    {
        private byte[] File;
        public Packet_File_Message(byte[] data) : base(data) 
        { 
            packet_type = Packet_Type.File_Message;
            File = data;
        }

        public override void Handle()
        {
            Window current_window = Global.Get_Current_Window();
            if (current_window == null) return;
            if (!(current_window is Chat_Room_Page)) return;
            Global.Run_Task_On_UI(new Task(() => ((Chat_Room_Page)current_window).Add_File(File, Controls.Message_Block.Text_Direction.LEFT)));
        }
        public byte[] Get_File() => File;
        public override byte[] Encode_Packet()
        {
            byte[] encoded = File;
            int length = encoded.Length;
            List<byte> result = new List<byte>();
            result.Add((byte)length);
            result.AddRange(encoded);
            return result.ToArray();
        }
        public override void Decode_Packet()
        {
            int length = File[0];
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
                bytes[i] = File[i + 1];
            File = bytes;
        }
    }
}
