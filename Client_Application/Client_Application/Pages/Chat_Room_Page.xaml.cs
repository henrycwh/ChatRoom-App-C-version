using Client_Application.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using static Client_Application.Controls.Message_Block;
using System.IO;

namespace Client_Application.Pages
{
    public partial class Chat_Room_Page : Window
    {
        public Chat_Room_Page()
        {
            InitializeComponent();
            Loaded += delegate
            {
                Add_Broadcast_Message("歡迎您的加入。");
            };
            Closing += delegate { Global.Disconnect_From_Server(); };
        }
        public void Add_Broadcast_Message(string Text) => Add_Text(Text, Text_Direction.CENTER);

        public void Add_Text(string Text, Text_Direction Direction)
        {
            Message_Block block = new Message_Block();
            block.Set_Text(Text);
            block.Set_Alignment(Direction);
            Message_Container.Children.Add(block);
        }
        public void Add_File(byte[] File, Text_Direction Direction)
        {
            Message_Block block = new Message_Block();
            block.Set_Text(File);
            block.Set_Alignment(Direction);
            Message_Container.Children.Add(block);
        }

        private void On_Key_Down(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Global.Get_Server().Send_Message(Input_Message.Text);
                Input_Message.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openfiledialog = new System.Windows.Forms.OpenFileDialog();
            if(openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Selected_File = openfiledialog.FileName;
                try
                {
                    // 使用 FileStream 來讀取二進制檔案
                    using (FileStream fileStream = new FileStream(Selected_File, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[fileStream.Length];
                        int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                        Global.Get_Server().Send_File(buffer);
                        // 在這裡可以處理讀取到的二進制資料，例如播放 mp3 檔案
                        MessageBox.Show($"成功讀取 {bytesRead} 位元組的 MP3 檔案。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"讀取 MP3 檔案失敗: {ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
