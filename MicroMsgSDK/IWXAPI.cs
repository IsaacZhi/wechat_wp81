using System;
using System.Threading.Tasks;
namespace MicroMsg.sdk
{
	public interface IWXAPI
	{
		void OpenWXApp();
		Task<bool> SendReq(BaseReq request, string targetAppID = "wechat");
		Task<bool> SendResp(BaseResp response, string targetAppID = "wechat");
	}
}
