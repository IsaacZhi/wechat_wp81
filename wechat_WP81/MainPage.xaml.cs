using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MicroMsg.sdk;
using Windows.Storage;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace wechat_WP81
{
    public class ConstString
    {
        //public const string APPID = "wxca942bbff22e0e51";//腾讯视频appid测试用
        public const string APPID = "wxd930ea5d5a258f4f";//sdk_demo的appid
        public const string SCOPE = "snsapi_userinfo,snsapi_friend,snsnapi_message,snsapi_contact";
        public const string STATE = "sdk_demo";
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        //所有发送数据的按钮的处理函数，根据点击不同，组织不同数据发送给微信
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("############################");
            System.Diagnostics.Debug.WriteLine("Button_Click_1 in");
            string AppID = ConstString.APPID;
            int scene = SendMessageToWX.Req.WXSceneSession; //发给微信朋友
            WXBaseMessage message = null;

            //拉起微信
            if (btnLaunch == sender)
            {
                WXAPIFactory.CreateWXAPI(AppID).OpenWXApp();
                return;
            }

            //读取实例数据，示例数据都是从res中读取，实现时根据你的应用实际情况取得数据
            byte[] png = await readRes("ms-appx:///Res/1.png");//缩略图
            byte[] jpg = await readRes("ms-appx:///Res/1.jpg");//图片
            byte[] gif = await readRes("ms-appx:///Res/1.gif");//动态表情
            byte[] txt = await readRes("ms-appx:///Res/1.txt");//文本
            byte[] doc = await readRes("ms-appx:///Res/1.doc");//word文件

            //发送到朋友圈，设置scene为WXSceneTimeline
            if (sender == btnTxtSns || sender == btnImgSns || sender == btnUrlSns || sender == btnMusicSns)
            {
                scene = SendMessageToWX.Req.WXSceneTimeline;//发送到朋友圈
            }

            //用户选择发给好友还是朋友圈，设置scene为WXSceneChooseByUser
            if (sender == btnTxt_ChooseByUser)
            {
                scene = SendMessageToWX.Req.WXSceneChooseByUser;
            }

            //文本数据的组装
            if (sender == btnTxt || sender == btnTxtSns)
            {
                WXTextMessage msg = new WXTextMessage();
                msg.Title = "文本";
                //msg.Description = "这是一个文本";
                msg.ThumbData = null;
                msg.Text = "这是一段文本内容";
                message = msg;
            }

            //图片数据的组装
            if (sender == btnImg || sender == btnImgSns)
            {
                WXImageMessage msg = new WXImageMessage();
                msg.Title = "图片";
                msg.Description = "这是一个图片";
                msg.ThumbData = png;

                //图片的byte[]数据
                //msg.ImageData = jpg;

                //图片的网络链接，ImageUrl和ImageData二者取其一，不要同时都填值
                msg.ImageUrl = "http://soso7.gtimg.cn/sosopic/0/4085165533549977883/150";

                message = msg;
            }

            //动态表情数据的组装
            if (sender == btnEmj)
            {
                WXEmojiMessage msg = new WXEmojiMessage();
                msg.Title = "Gif";
                msg.Description = "这是一个Gif";
                msg.ThumbData = png;

                msg.EmojiData = gif;

                message = msg;
            }

            //文件数据的组装
            if (sender == btnFile)
            {
                WXFileMessage msg = new WXFileMessage();
                msg.Title = "微信SDK需求";
                msg.Description = "这是文件描述了微信SDK需求";
                msg.ThumbData = png;

                msg.FileData = doc;
                msg.FileName = "1.doc";

                message = msg;
            }

            //链接数据的组装
            if (sender == btnUrl || sender == btnUrlSns)
            {
                WXWebpageMessage msg = new WXWebpageMessage();
                msg.Title = "QQ首页";
                msg.Description = "打开QQ首页";
                msg.ThumbData = png;

                msg.WebpageUrl = "http://www.qq.com";

                message = msg;
            }

            //音乐数据的组装
            if (sender == btnMusic || sender == btnMusicSns || sender == btnTxt_ChooseByUser)
            {
                WXMusicMessage msg = new WXMusicMessage();
                msg.Title = "阿士匹灵";
                msg.Description = "陈奕迅";
                msg.ThumbData = png;

                msg.MusicUrl = "http://y.qq.com/i/song.html#p=7B22736F6E675F4E616D65223A2220E998BFE5A3ABE58CB9E781B5222C22736F6E675F5761704C69766555524C223A22687474703A2F2F74736D7573696334382E74632E71712E636F6D2F586B30305156374F4141414134674141414250657376594E4D7A55327A4441784E654D35586A4A63366F76484F415942443174754251414F4347384439394B343965776F6244733D2F31313134383332342E6D34613F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D30222C22736F6E675F5769666955524C223A22687474703A2F2F73747265616D31312E71716D757369632E71712E636F6D2F33313134383332342E6D7033222C226E657454797065223A2277696669222C22736F6E675F416C62756D223A22E88BB1E79A87E7B2BEE9808932303038222C22736F6E675F4944223A313134383332342C22736F6E675F54797065223A312C22736F6E675F53696E676572223A22E99988E5A595E8BF85222C22736F6E675F576170446F776E4C6F616455524C223A22687474703A2F2F74736D757369633132382E74632E71712E636F6D2F586C464E4D31376C414141415977414141442F6954452B656E7A457A6D6A59324D4D55314F444C5230483937504B344142713850446D3550476C56436366394D536661644D5464704E7A3163665451344D673D3D2F33313134383332342E6D70333F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D3026616D703B73747265616D5F706F733D35227D";
                msg.MusicLowBandUrl = "http://y.qq.com/i/song.html#p=7B22736F6E675F4E616D65223A2220E998BFE5A3ABE58CB9E781B5222C22736F6E675F5761704C69766555524C223A22687474703A2F2F74736D7573696334382E74632E71712E636F6D2F586B30305156374F4141414134674141414250657376594E4D7A55327A4441784E654D35586A4A63366F76484F415942443174754251414F4347384439394B343965776F6244733D2F31313134383332342E6D34613F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D30222C22736F6E675F5769666955524C223A22687474703A2F2F73747265616D31312E71716D757369632E71712E636F6D2F33313134383332342E6D7033222C226E657454797065223A2263617272696572222C22736F6E675F416C62756D223A22E88BB1E79A87E7B2BEE9808932303038222C22736F6E675F4944223A313134383332342C22736F6E675F54797065223A312C22736F6E675F53696E676572223A22E99988E5A595E8BF85222C22736F6E675F576170446F776E4C6F616455524C223A22687474703A2F2F74736D757369633132382E74632E71712E636F6D2F586C464E4D31376C414141415977414141442F6954452B656E7A457A6D6A59324D4D55314F444C5230483937504B344142713850446D3550476C56436366394D536661644D5464704E7A3163665451344D673D3D2F33313134383332342E6D70333F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D3026616D703B73747265616D5F706F733D35227D";

                message = msg;
            }

            //视频数据的组装
            if (sender == btnVedio)
            {
                WXVideoMessage msg = new WXVideoMessage();
                msg.Title = "阿士匹灵";
                msg.Description = "陈奕迅";
                msg.ThumbData = png;

                msg.VideoUrl = "http://y.qq.com/i/song.html#p=7B22736F6E675F4E616D65223A2220E998BFE5A3ABE58CB9E781B5222C22736F6E675F5761704C69766555524C223A22687474703A2F2F74736D7573696334382E74632E71712E636F6D2F586B30305156374F4141414134674141414250657376594E4D7A55327A4441784E654D35586A4A63366F76484F415942443174754251414F4347384439394B343965776F6244733D2F31313134383332342E6D34613F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D30222C22736F6E675F5769666955524C223A22687474703A2F2F73747265616D31312E71716D757369632E71712E636F6D2F33313134383332342E6D7033222C226E657454797065223A2277696669222C22736F6E675F416C62756D223A22E88BB1E79A87E7B2BEE9808932303038222C22736F6E675F4944223A313134383332342C22736F6E675F54797065223A312C22736F6E675F53696E676572223A22E99988E5A595E8BF85222C22736F6E675F576170446F776E4C6F616455524C223A22687474703A2F2F74736D757369633132382E74632E71712E636F6D2F586C464E4D31376C414141415977414141442F6954452B656E7A457A6D6A59324D4D55314F444C5230483937504B344142713850446D3550476C56436366394D536661644D5464704E7A3163665451344D673D3D2F33313134383332342E6D70333F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D3026616D703B73747265616D5F706F733D35227D";
                msg.VideoLowBandUrl = "http://y.qq.com/i/song.html#p=7B22736F6E675F4E616D65223A2220E998BFE5A3ABE58CB9E781B5222C22736F6E675F5761704C69766555524C223A22687474703A2F2F74736D7573696334382E74632E71712E636F6D2F586B30305156374F4141414134674141414250657376594E4D7A55327A4441784E654D35586A4A63366F76484F415942443174754251414F4347384439394B343965776F6244733D2F31313134383332342E6D34613F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D30222C22736F6E675F5769666955524C223A22687474703A2F2F73747265616D31312E71716D757369632E71712E636F6D2F33313134383332342E6D7033222C226E657454797065223A2263617272696572222C22736F6E675F416C62756D223A22E88BB1E79A87E7B2BEE9808932303038222C22736F6E675F4944223A313134383332342C22736F6E675F54797065223A312C22736F6E675F53696E676572223A22E99988E5A595E8BF85222C22736F6E675F576170446F776E4C6F616455524C223A22687474703A2F2F74736D757369633132382E74632E71712E636F6D2F586C464E4D31376C414141415977414141442F6954452B656E7A457A6D6A59324D4D55314F444C5230483937504B344142713850446D3550476C56436366394D536661644D5464704E7A3163665451344D673D3D2F33313134383332342E6D70333F7569643D3233343931313534393826616D703B63743D3026616D703B636869643D3026616D703B73747265616D5F706F733D35227D";

                message = msg;
            }

            //把数据发送给微信，数据错误或发送错误，会抛出WXException，调试时，可从WXException中得知是什么错误。
            //发布应用时，可直接向用户提示错误，也可以根据自己实际需要定制错误的处理
            try
            {
                SendMessageToWX.Req req = new SendMessageToWX.Req(message, scene);
                IWXAPI api = WXAPIFactory.CreateWXAPI(AppID);

                //Console.WriteLine("api.SendReq in");
                api.SendReq(req);
            }
            catch (WXException ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        //工具函数，读取res资源成byte数组
        private async Task<byte[]> readRes(string path)
        {
            /*StreamResourceInfo info = App.GetResourceStream(new Uri(path, UriKind.Relative));
            if (info == null) return null;
            Stream stream = info.Stream;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;*/
            Uri dataUri = new Uri(path);

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            using (Stream f = await file.OpenStreamForReadAsync())
            {
                byte[] data = new byte[f.Length];
                f.Read(data, 0, (int)f.Length);
                return data;
            }
        }
    }
}
