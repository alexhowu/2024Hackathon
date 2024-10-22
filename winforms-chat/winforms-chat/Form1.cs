using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using winforms_chat.ChatForm;

namespace winforms_chat
{
	public partial class Form1 : Form
	{
		private ChatForm.Chatbox chat_panel;

        public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ChatForm.ChatboxInfo cbi = new ChatForm.ChatboxInfo();
			cbi.NamePlaceholder = "Sage300 Intelligence";
            //cbi.NamePlaceholder = "testing";
            //cbi.PhonePlaceholder = "(111) 222-3333";

            chat_panel = new ChatForm.Chatbox(cbi);
			chat_panel.Name = "chat_panel";
			chat_panel.Dock = DockStyle.Fill;
			this.Controls.Add(chat_panel);
		}

		public void AddMessage(string message, bool isAgent = true)
		{
			var textChatModel = new TextChatModel
			{
				Inbound = isAgent,
				Author = isAgent ? "Intellengence" : "You",
				Body = message,
				Time = DateTime.Now
			};
			chat_panel.AddMessage(textChatModel);
		}

		public event EventHandler<string> UserMessage;

		public void OnUserMessage(string message)
		{
			UserMessage?.Invoke(this, message);
		}
	}
}
