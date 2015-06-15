//using Microsoft.Phone.Controls;
using System;
using System.Windows;
//using System.Windows.Navigation;
//using Windows.Phone.Storage.SharedAccess;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace MicroMsg.sdk
{
	public class WXEntryBasePage : /*PhoneApplicationPage*/ Page
	{
		private bool bIsHandled;
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (!this.bIsHandled)
			{
				e.Uri.ToString();
				string text = null;
                string navigationContext = (string)e.Content;
				/*if (base.get_NavigationContext().get_QueryString().get_Keys().Contains("fileToken"))
				{
					text = base.get_NavigationContext().get_QueryString().get_Item("fileToken");
				}*/
                if ( navigationContext.Contains("fileToken"))
                {
                    //WwwFormUrlDecoder(navigationContext);
                    
                }
				if (!string.IsNullOrEmpty(text))
				{
					this.parseData(text);
				}
				this.bIsHandled = true;
			}
		}
		private /*async*/ void parseData(string fileToken)
		{
            /*
			try
			{
				StorageFolder localFolder = ApplicationData.get_Current().get_LocalFolder();
				if (!FileUtil.dirExists("wechat_sdk"))
				{
					FileUtil.createDir("wechat_sdk");
				}
				await SharedStorageAccessManager.CopySharedFileAsync(localFolder, "wechat_sdk\\wp.wechat", 1, fileToken);
				if (FileUtil.fileExists("wechat_sdk\\wp.wechat"))
				{
					TransactData transactData = TransactData.ReadFromFile("wechat_sdk\\wp.wechat");
					if (!transactData.ValidateData(true))
					{
						//MessageBox.Show("数据验证失败");
                        await new MessageDialog("数据验证失败").ShowAsync();
					}
					else
					{
						if (!transactData.CheckSupported())
						{
							//MessageBox.Show("当前版本不支持该请求");
                            await new MessageDialog("当前版本不支持该请求").ShowAsync();
						}
						else
						{
							if (transactData.Req != null)
							{
								if (transactData.Req.Type() == 3)
								{
									this.On_GetMessageFromWX_Request(transactData.Req as GetMessageFromWX.Req);
								}
								else
								{
									if (transactData.Req.Type() == 4)
									{
										this.On_ShowMessageFromWX_Request(transactData.Req as ShowMessageFromWX.Req);
									}
								}
							}
							else
							{
								if (transactData.Resp != null)
								{
									if (transactData.Resp.Type() == 2)
									{
										this.On_SendMessageToWX_Response(transactData.Resp as SendMessageToWX.Resp);
									}
									else
									{
										if (transactData.Resp.Type() == 1)
										{
											this.On_SendAuth_Response(transactData.Resp as SendAuth.Resp);
										}
										else
										{
											if (transactData.Resp.Type() == 5)
											{
												this.On_SendPay_Response(transactData.Resp as SendPay.Resp);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
                await new MessageDialog(ex.Message).ShowAsync();
			}
            */
		}
		public virtual void On_GetMessageFromWX_Request(GetMessageFromWX.Req request)
		{
		}
		public virtual void On_SendMessageToWX_Response(SendMessageToWX.Resp response)
		{
		}
		public virtual void On_SendAuth_Response(SendAuth.Resp response)
		{
		}
		public virtual void On_ShowMessageFromWX_Request(ShowMessageFromWX.Req request)
		{
		}
		public virtual void On_SendPay_Response(SendPay.Resp response)
		{
		}
	}
}
