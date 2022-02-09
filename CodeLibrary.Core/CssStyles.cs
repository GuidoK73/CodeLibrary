﻿using System;
using System.IO;
using System.Text;

namespace CodeLibrary.Core
{
    public enum CssStyle
    {
        None = 0,
        Splendor = 1,
        Modest = 2,
        Retro = 3,
        Air = 4
    }

    public static class CssStyles
    {
        public static string GetCSS(CssStyle style)
        {
            switch (style)
            {
                case CssStyle.Air:
                    return AirCSS();

                case CssStyle.Modest:
                    return ModestCSS();

                case CssStyle.Splendor:
                    return SplendorCSS();

                case CssStyle.Retro:
                    return RetroCSS();

                case CssStyle.None:
                    return string.Empty;
            }
            return String.Empty;
        }

        private static string SplendorCSS()
        {
            string _result = "QG1lZGlhIHByaW50IHsNCiAgKiwNCiAgKjpiZWZvcmUsDQogICo6YWZ0ZXIgew0KICAgIGJhY2tncm91bmQ6IHRyYW5zcGFyZW50ICFpbXBvcnRhbnQ7DQogICAgY29sb3I6ICMwMDAgIWltcG9ydGFudDsNCiAgICBib3gtc2hhZG93OiBub25lICFpbXBvcnRhbnQ7DQogICAgdGV4dC1zaGFkb3c6IG5vbmUgIWltcG9ydGFudDsNCiAgfQ0KDQogIGEsDQogIGE6dmlzaXRlZCB7DQogICAgdGV4dC1kZWNvcmF0aW9uOiB1bmRlcmxpbmU7DQogIH0NCg0KICBhW2hyZWZdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiICgiIGF0dHIoaHJlZikgIikiOw0KICB9DQoNCiAgYWJiclt0aXRsZV06YWZ0ZXIgew0KICAgIGNvbnRlbnQ6ICIgKCIgYXR0cih0aXRsZSkgIikiOw0KICB9DQoNCiAgYVtocmVmXj0iIyJdOmFmdGVyLA0KICBhW2hyZWZePSJqYXZhc2NyaXB0OiJdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiIjsNCiAgfQ0KDQogIHByZSwNCiAgYmxvY2txdW90ZSB7DQogICAgYm9yZGVyOiAxcHggc29saWQgIzk5OTsNCiAgICBwYWdlLWJyZWFrLWluc2lkZTogYXZvaWQ7DQogIH0NCg0KICB0aGVhZCB7DQogICAgZGlzcGxheTogdGFibGUtaGVhZGVyLWdyb3VwOw0KICB9DQoNCiAgdHIsDQogIGltZyB7DQogICAgcGFnZS1icmVhay1pbnNpZGU6IGF2b2lkOw0KICB9DQoNCiAgaW1nIHsNCiAgICBtYXgtd2lkdGg6IDEwMCUgIWltcG9ydGFudDsNCiAgfQ0KDQogIHAsDQogIGgyLA0KICBoMyB7DQogICAgb3JwaGFuczogMzsNCiAgICB3aWRvd3M6IDM7DQogIH0NCg0KICBoMiwNCiAgaDMgew0KICAgIHBhZ2UtYnJlYWstYWZ0ZXI6IGF2b2lkOw0KICB9DQp9DQoNCmh0bWwgew0KICBmb250LXNpemU6IDEycHg7DQp9DQoNCkBtZWRpYSBzY3JlZW4gYW5kIChtaW4td2lkdGg6IDMycmVtKSBhbmQgKG1heC13aWR0aDogNDhyZW0pIHsNCiAgaHRtbCB7DQogICAgZm9udC1zaXplOiAxNXB4Ow0KICB9DQp9DQoNCkBtZWRpYSBzY3JlZW4gYW5kIChtaW4td2lkdGg6IDQ4cmVtKSB7DQogIGh0bWwgew0KICAgIGZvbnQtc2l6ZTogMTZweDsNCiAgfQ0KfQ0KDQpib2R5IHsNCiAgbGluZS1oZWlnaHQ6IDEuODU7DQp9DQoNCnAsDQouc3BsZW5kb3ItcCB7DQogIGZvbnQtc2l6ZTogMXJlbTsNCiAgbWFyZ2luLWJvdHRvbTogMS4zcmVtOw0KfQ0KDQpoMSwNCi5zcGxlbmRvci1oMSwNCmgyLA0KLnNwbGVuZG9yLWgyLA0KaDMsDQouc3BsZW5kb3ItaDMsDQpoNCwNCi5zcGxlbmRvci1oNCB7DQogIG1hcmdpbjogMS40MTRyZW0gMCAuNXJlbTsNCiAgZm9udC13ZWlnaHQ6IGluaGVyaXQ7DQogIGxpbmUtaGVpZ2h0OiAxLjQyOw0KfQ0KDQpoMSwNCi5zcGxlbmRvci1oMSB7DQogIG1hcmdpbi10b3A6IDA7DQogIGZvbnQtc2l6ZTogMy45OThyZW07DQp9DQoNCmgyLA0KLnNwbGVuZG9yLWgyIHsNCiAgZm9udC1zaXplOiAyLjgyN3JlbTsNCn0NCg0KaDMsDQouc3BsZW5kb3ItaDMgew0KICBmb250LXNpemU6IDEuOTk5cmVtOw0KfQ0KDQpoNCwNCi5zcGxlbmRvci1oNCB7DQogIGZvbnQtc2l6ZTogMS40MTRyZW07DQp9DQoNCmg1LA0KLnNwbGVuZG9yLWg1IHsNCiAgZm9udC1zaXplOiAxLjEyMXJlbTsNCn0NCg0KaDYsDQouc3BsZW5kb3ItaDYgew0KICBmb250LXNpemU6IC44OHJlbTsNCn0NCg0Kc21hbGwsDQouc3BsZW5kb3Itc21hbGwgew0KICBmb250LXNpemU6IC43MDdlbTsNCn0NCg0KLyogaHR0cHM6Ly9naXRodWIuY29tL21ybXJzL2ZsdWlkaXR5ICovDQoNCmltZywNCmNhbnZhcywNCmlmcmFtZSwNCnZpZGVvLA0Kc3ZnLA0Kc2VsZWN0LA0KdGV4dGFyZWEgew0KICBtYXgtd2lkdGg6IDEwMCU7DQp9DQoNCkBpbXBvcnQgdXJsKGh0dHA6Ly9mb250cy5nb29nbGVhcGlzLmNvbS9jc3M/ZmFtaWx5PU1lcnJpd2VhdGhlcjozMDBpdGFsaWMsMzAwKTsNCg0KaHRtbCB7DQogIGZvbnQtc2l6ZTogMThweDsNCiAgbWF4LXdpZHRoOiAxMDAlOw0KfQ0KDQpib2R5IHsNCiAgY29sb3I6ICM0NDQ7DQogIGZvbnQtZmFtaWx5OiAnTWVycml3ZWF0aGVyJywgR2VvcmdpYSwgc2VyaWY7DQogIG1hcmdpbjogMDsNCiAgbWF4LXdpZHRoOiAxMDAlOw0KfQ0KDQovKiA9PT0gQSBiaXQgb2YgYSBncm9zcyBoYWNrIHNvIHdlIGNhbiBoYXZlIGJsZWVkaW5nIGRpdnMvYmxvY2txdW90ZXMuICovDQoNCnAsDQoqOm5vdChkaXYpOm5vdChpbWcpOm5vdChib2R5KTpub3QoaHRtbCk6bm90KGxpKTpub3QoYmxvY2txdW90ZSk6bm90KHApIHsNCiAgbWFyZ2luOiAxcmVtIGF1dG8gMXJlbTsNCiAgbWF4LXdpZHRoOiAzNnJlbTsNCiAgcGFkZGluZzogLjI1cmVtOw0KfQ0KDQpkaXYgew0KICB3aWR0aDogMTAwJTsNCn0NCg0KZGl2IGltZyB7DQogIHdpZHRoOiAxMDAlOw0KfQ0KDQpibG9ja3F1b3RlIHAgew0KICBmb250LXNpemU6IDEuNXJlbTsNCiAgZm9udC1zdHlsZTogaXRhbGljOw0KICBtYXJnaW46IDFyZW0gYXV0byAxcmVtOw0KICBtYXgtd2lkdGg6IDQ4cmVtOw0KfQ0KDQpsaSB7DQogIG1hcmdpbi1sZWZ0OiAycmVtOw0KfQ0KDQovKiBDb3VudGVyYWN0IHRoZSBzcGVjaWZpY2l0eSBvZiB0aGUgZ3Jvc3MgKjpub3QoKSBjaGFpbi4gKi8NCg0KaDEgew0KICBwYWRkaW5nOiA0cmVtIDAgIWltcG9ydGFudDsNCn0NCg0KLyogID09PSBFbmQgZ3Jvc3MgaGFjayAqLw0KDQpwIHsNCiAgY29sb3I6ICM1NTU7DQogIGhlaWdodDogYXV0bzsNCiAgbGluZS1oZWlnaHQ6IDEuNDU7DQp9DQoNCnByZSwNCmNvZGUgew0KICBmb250LWZhbWlseTogTWVubG8sIE1vbmFjbywgIkNvdXJpZXIgTmV3IiwgbW9ub3NwYWNlOw0KfQ0KDQpwcmUgew0KICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmFmYWZhOw0KICBmb250LXNpemU6IC44cmVtOw0KICBvdmVyZmxvdy14OiBzY3JvbGw7DQogIHBhZGRpbmc6IDEuMTI1ZW07DQp9DQoNCmEsDQphOnZpc2l0ZWQgew0KICBjb2xvcjogIzM0OThkYjsNCn0NCg0KYTpob3ZlciwNCmE6Zm9jdXMsDQphOmFjdGl2ZSB7DQogIGNvbG9yOiAjMjk4MGI5Ow0KfQ==";
            return StreamToString(Base64StringToStream(_result));
        }

        private static string ModestCSS()
        {
            string _result = @"QG1lZGlhIHByaW50IHsNCiAgKiwNCiAgKjpiZWZvcmUsDQogICo6YWZ0ZXIgew0KICAgIGJhY2tncm91bmQ6IHRyYW5zcGFyZW50ICFpbXBvcnRhbnQ7DQogICAgY29sb3I6ICMwMDAgIWltcG9ydGFudDsNCiAgICBib3gtc2hhZG93OiBub25lICFpbXBvcnRhbnQ7DQogICAgdGV4dC1zaGFkb3c6IG5vbmUgIWltcG9ydGFudDsNCiAgfQ0KDQogIGEsDQogIGE6dmlzaXRlZCB7DQogICAgdGV4dC1kZWNvcmF0aW9uOiB1bmRlcmxpbmU7DQogIH0NCg0KICBhW2hyZWZdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiICgiIGF0dHIoaHJlZikgIikiOw0KICB9DQoNCiAgYWJiclt0aXRsZV06YWZ0ZXIgew0KICAgIGNvbnRlbnQ6ICIgKCIgYXR0cih0aXRsZSkgIikiOw0KICB9DQoNCiAgYVtocmVmXj0iIyJdOmFmdGVyLA0KICBhW2hyZWZePSJqYXZhc2NyaXB0OiJdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiIjsNCiAgfQ0KDQogIHByZSwNCiAgYmxvY2txdW90ZSB7DQogICAgYm9yZGVyOiAxcHggc29saWQgIzk5OTsNCiAgICBwYWdlLWJyZWFrLWluc2lkZTogYXZvaWQ7DQogIH0NCg0KICB0aGVhZCB7DQogICAgZGlzcGxheTogdGFibGUtaGVhZGVyLWdyb3VwOw0KICB9DQoNCiAgdHIsDQogIGltZyB7DQogICAgcGFnZS1icmVhay1pbnNpZGU6IGF2b2lkOw0KICB9DQoNCiAgaW1nIHsNCiAgICBtYXgtd2lkdGg6IDEwMCUgIWltcG9ydGFudDsNCiAgfQ0KDQogIHAsDQogIGgyLA0KICBoMyB7DQogICAgb3JwaGFuczogMzsNCiAgICB3aWRvd3M6IDM7DQogIH0NCg0KICBoMiwNCiAgaDMgew0KICAgIHBhZ2UtYnJlYWstYWZ0ZXI6IGF2b2lkOw0KICB9DQp9DQoNCnByZSwNCmNvZGUgew0KICBmb250LWZhbWlseTogTWVubG8sIE1vbmFjbywgIkNvdXJpZXIgTmV3IiwgbW9ub3NwYWNlOw0KfQ0KDQpwcmUgew0KICBwYWRkaW5nOiAuNXJlbTsNCiAgbGluZS1oZWlnaHQ6IDEuMjU7DQogIG92ZXJmbG93LXg6IHNjcm9sbDsNCn0NCg0KYSwNCmE6dmlzaXRlZCB7DQogIGNvbG9yOiAjMzQ5OGRiOw0KfQ0KDQphOmhvdmVyLA0KYTpmb2N1cywNCmE6YWN0aXZlIHsNCiAgY29sb3I6ICMyOTgwYjk7DQp9DQoNCi5tb2Rlc3Qtbm8tZGVjb3JhdGlvbiB7DQogIHRleHQtZGVjb3JhdGlvbjogbm9uZTsNCn0NCg0KaHRtbCB7DQogIGZvbnQtc2l6ZTogMTJweDsNCn0NCg0KQG1lZGlhIHNjcmVlbiBhbmQgKG1pbi13aWR0aDogMzJyZW0pIGFuZCAobWF4LXdpZHRoOiA0OHJlbSkgew0KICBodG1sIHsNCiAgICBmb250LXNpemU6IDE1cHg7DQogIH0NCn0NCg0KQG1lZGlhIHNjcmVlbiBhbmQgKG1pbi13aWR0aDogNDhyZW0pIHsNCiAgaHRtbCB7DQogICAgZm9udC1zaXplOiAxNnB4Ow0KICB9DQp9DQoNCmJvZHkgew0KICBsaW5lLWhlaWdodDogMS44NTsNCn0NCg0KcCwNCi5tb2Rlc3QtcCB7DQogIGZvbnQtc2l6ZTogMXJlbTsNCiAgbWFyZ2luLWJvdHRvbTogMS4zcmVtOw0KfQ0KDQpoMSwNCi5tb2Rlc3QtaDEsDQpoMiwNCi5tb2Rlc3QtaDIsDQpoMywNCi5tb2Rlc3QtaDMsDQpoNCwNCi5tb2Rlc3QtaDQgew0KICBtYXJnaW46IDEuNDE0cmVtIDAgLjVyZW07DQogIGZvbnQtd2VpZ2h0OiBpbmhlcml0Ow0KICBsaW5lLWhlaWdodDogMS40MjsNCn0NCg0KaDEsDQoubW9kZXN0LWgxIHsNCiAgbWFyZ2luLXRvcDogMDsNCiAgZm9udC1zaXplOiAzLjk5OHJlbTsNCn0NCg0KaDIsDQoubW9kZXN0LWgyIHsNCiAgZm9udC1zaXplOiAyLjgyN3JlbTsNCn0NCg0KaDMsDQoubW9kZXN0LWgzIHsNCiAgZm9udC1zaXplOiAxLjk5OXJlbTsNCn0NCg0KaDQsDQoubW9kZXN0LWg0IHsNCiAgZm9udC1zaXplOiAxLjQxNHJlbTsNCn0NCg0KaDUsDQoubW9kZXN0LWg1IHsNCiAgZm9udC1zaXplOiAxLjEyMXJlbTsNCn0NCg0KaDYsDQoubW9kZXN0LWg2IHsNCiAgZm9udC1zaXplOiAuODhyZW07DQp9DQoNCnNtYWxsLA0KLm1vZGVzdC1zbWFsbCB7DQogIGZvbnQtc2l6ZTogLjcwN2VtOw0KfQ0KDQovKiBodHRwczovL2dpdGh1Yi5jb20vbXJtcnMvZmx1aWRpdHkgKi8NCg0KaW1nLA0KY2FudmFzLA0KaWZyYW1lLA0KdmlkZW8sDQpzdmcsDQpzZWxlY3QsDQp0ZXh0YXJlYSB7DQogIG1heC13aWR0aDogMTAwJTsNCn0NCg0KQGltcG9ydCB1cmwoaHR0cDovL2ZvbnRzLmdvb2dsZWFwaXMuY29tL2Nzcz9mYW1pbHk9T3BlbitTYW5zK0NvbmRlbnNlZDozMDAsMzAwaXRhbGljLDcwMCk7DQoNCkBpbXBvcnQgdXJsKGh0dHA6Ly9mb250cy5nb29nbGVhcGlzLmNvbS9jc3M/ZmFtaWx5PUFyaW1vOjcwMCw3MDBpdGFsaWMpOw0KDQpodG1sIHsNCiAgZm9udC1zaXplOiAxOHB4Ow0KICBtYXgtd2lkdGg6IDEwMCU7DQp9DQoNCmJvZHkgew0KICBjb2xvcjogIzQ0NDsNCiAgZm9udC1mYW1pbHk6ICdPcGVuIFNhbnMgQ29uZGVuc2VkJywgc2Fucy1zZXJpZjsNCiAgZm9udC13ZWlnaHQ6IDMwMDsNCiAgbWFyZ2luOiAwIGF1dG87DQogIG1heC13aWR0aDogNDhyZW07DQogIGxpbmUtaGVpZ2h0OiAxLjQ1Ow0KICBwYWRkaW5nOiAuMjVyZW07DQp9DQoNCmgxLA0KaDIsDQpoMywNCmg0LA0KaDUsDQpoNiB7DQogIGZvbnQtZmFtaWx5OiBBcmltbywgSGVsdmV0aWNhLCBzYW5zLXNlcmlmOw0KfQ0KDQpoMSwNCmgyLA0KaDMgew0KICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2ZhZmFmYTsNCiAgbWFyZ2luLWJvdHRvbTogMS4xNXJlbTsNCiAgcGFkZGluZy1ib3R0b206IC41cmVtOw0KICB0ZXh0LWFsaWduOiBjZW50ZXI7DQp9DQoNCmJsb2NrcXVvdGUgew0KICBib3JkZXItbGVmdDogOHB4IHNvbGlkICNmYWZhZmE7DQogIHBhZGRpbmc6IDFyZW07DQp9DQoNCnByZSwNCmNvZGUgew0KICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmFmYWZhOw0KfQ==";
            return StreamToString(Base64StringToStream(_result));
        }

        public static string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                stream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static Stream Base64StringToStream(string base64String)
        {
            var memorySteam = new MemoryStream(Convert.FromBase64String(base64String));
            memorySteam.Seek(0, SeekOrigin.Begin);
            return memorySteam;
        }

        private static string RetroCSS()
        {
            string _result = "DQoNCnByZSwNCmNvZGUgew0KICBmb250LWZhbWlseTogTWVubG8sIE1vbmFjbywgIkNvdXJpZXIgTmV3IiwgbW9ub3NwYWNlOw0KfQ0KDQpwcmUgew0KICBwYWRkaW5nOiAuNXJlbTsNCiAgbGluZS1oZWlnaHQ6IDEuMjU7DQogIG92ZXJmbG93LXg6IHNjcm9sbDsNCn0NCg0KQG1lZGlhIHByaW50IHsNCiAgKiwNCiAgKjpiZWZvcmUsDQogICo6YWZ0ZXIgew0KICAgIGJhY2tncm91bmQ6IHRyYW5zcGFyZW50ICFpbXBvcnRhbnQ7DQogICAgY29sb3I6ICMwMDAgIWltcG9ydGFudDsNCiAgICBib3gtc2hhZG93OiBub25lICFpbXBvcnRhbnQ7DQogICAgdGV4dC1zaGFkb3c6IG5vbmUgIWltcG9ydGFudDsNCiAgfQ0KDQogIGEsDQogIGE6dmlzaXRlZCB7DQogICAgdGV4dC1kZWNvcmF0aW9uOiB1bmRlcmxpbmU7DQogIH0NCg0KICBhW2hyZWZdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiICgiIGF0dHIoaHJlZikgIikiOw0KICB9DQoNCiAgYWJiclt0aXRsZV06YWZ0ZXIgew0KICAgIGNvbnRlbnQ6ICIgKCIgYXR0cih0aXRsZSkgIikiOw0KICB9DQoNCiAgYVtocmVmXj0iIyJdOmFmdGVyLA0KICBhW2hyZWZePSJqYXZhc2NyaXB0OiJdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiIjsNCiAgfQ0KDQogIHByZSwNCiAgYmxvY2txdW90ZSB7DQogICAgYm9yZGVyOiAxcHggc29saWQgIzk5OTsNCiAgICBwYWdlLWJyZWFrLWluc2lkZTogYXZvaWQ7DQogIH0NCg0KICB0aGVhZCB7DQogICAgZGlzcGxheTogdGFibGUtaGVhZGVyLWdyb3VwOw0KICB9DQoNCiAgdHIsDQogIGltZyB7DQogICAgcGFnZS1icmVhay1pbnNpZGU6IGF2b2lkOw0KICB9DQoNCiAgaW1nIHsNCiAgICBtYXgtd2lkdGg6IDEwMCUgIWltcG9ydGFudDsNCiAgfQ0KDQogIHAsDQogIGgyLA0KICBoMyB7DQogICAgb3JwaGFuczogMzsNCiAgICB3aWRvd3M6IDM7DQogIH0NCg0KICBoMiwNCiAgaDMgew0KICAgIHBhZ2UtYnJlYWstYWZ0ZXI6IGF2b2lkOw0KICB9DQp9DQoNCmEsDQphOnZpc2l0ZWQgew0KICBjb2xvcjogIzAxZmY3MDsNCn0NCg0KYTpob3ZlciwNCmE6Zm9jdXMsDQphOmFjdGl2ZSB7DQogIGNvbG9yOiAjMmVjYzQwOw0KfQ0KDQoucmV0cm8tbm8tZGVjb3JhdGlvbiB7DQogIHRleHQtZGVjb3JhdGlvbjogbm9uZTsNCn0NCg0KaHRtbCB7DQogIGZvbnQtc2l6ZTogMTJweDsNCn0NCg0KQG1lZGlhIHNjcmVlbiBhbmQgKG1pbi13aWR0aDogMzJyZW0pIGFuZCAobWF4LXdpZHRoOiA0OHJlbSkgew0KICBodG1sIHsNCiAgICBmb250LXNpemU6IDE1cHg7DQogIH0NCn0NCg0KQG1lZGlhIHNjcmVlbiBhbmQgKG1pbi13aWR0aDogNDhyZW0pIHsNCiAgaHRtbCB7DQogICAgZm9udC1zaXplOiAxNnB4Ow0KICB9DQp9DQoNCmJvZHkgew0KICBsaW5lLWhlaWdodDogMS44NTsNCn0NCg0KcCwNCi5yZXRyby1wIHsNCiAgZm9udC1zaXplOiAxcmVtOw0KICBtYXJnaW4tYm90dG9tOiAxLjNyZW07DQp9DQoNCmgxLA0KLnJldHJvLWgxLA0KaDIsDQoucmV0cm8taDIsDQpoMywNCi5yZXRyby1oMywNCmg0LA0KLnJldHJvLWg0IHsNCiAgbWFyZ2luOiAxLjQxNHJlbSAwIC41cmVtOw0KICBmb250LXdlaWdodDogaW5oZXJpdDsNCiAgbGluZS1oZWlnaHQ6IDEuNDI7DQp9DQoNCmgxLA0KLnJldHJvLWgxIHsNCiAgbWFyZ2luLXRvcDogMDsNCiAgZm9udC1zaXplOiAzLjk5OHJlbTsNCn0NCg0KaDIsDQoucmV0cm8taDIgew0KICBmb250LXNpemU6IDIuODI3cmVtOw0KfQ0KDQpoMywNCi5yZXRyby1oMyB7DQogIGZvbnQtc2l6ZTogMS45OTlyZW07DQp9DQoNCmg0LA0KLnJldHJvLWg0IHsNCiAgZm9udC1zaXplOiAxLjQxNHJlbTsNCn0NCg0KaDUsDQoucmV0cm8taDUgew0KICBmb250LXNpemU6IDEuMTIxcmVtOw0KfQ0KDQpoNiwNCi5yZXRyby1oNiB7DQogIGZvbnQtc2l6ZTogLjg4cmVtOw0KfQ0KDQpzbWFsbCwNCi5yZXRyby1zbWFsbCB7DQogIGZvbnQtc2l6ZTogLjcwN2VtOw0KfQ0KDQovKiBodHRwczovL2dpdGh1Yi5jb20vbXJtcnMvZmx1aWRpdHkgKi8NCg0KaW1nLA0KY2FudmFzLA0KaWZyYW1lLA0KdmlkZW8sDQpzdmcsDQpzZWxlY3QsDQp0ZXh0YXJlYSB7DQogIG1heC13aWR0aDogMTAwJTsNCn0NCg0KaHRtbCwNCmJvZHkgew0KICBiYWNrZ3JvdW5kLWNvbG9yOiAjMjIyOw0KICBtaW4taGVpZ2h0OiAxMDAlOw0KfQ0KDQpodG1sIHsNCiAgZm9udC1zaXplOiAxOHB4Ow0KfQ0KDQpib2R5IHsNCiAgY29sb3I6ICNmYWZhZmE7DQogIGZvbnQtZmFtaWx5OiAiQ291cmllciBOZXciOw0KICBsaW5lLWhlaWdodDogMS40NTsNCiAgbWFyZ2luOiA2cmVtIGF1dG8gMXJlbTsNCiAgbWF4LXdpZHRoOiA0OHJlbTsNCiAgcGFkZGluZzogLjI1cmVtOw0KfQ0KDQpwcmUgew0KICBiYWNrZ3JvdW5kLWNvbG9yOiAjMzMzOw0KfQ0KDQpibG9ja3F1b3RlIHsNCiAgYm9yZGVyLWxlZnQ6IDNweCBzb2xpZCAjMDFmZjcwOw0KICBwYWRkaW5nLWxlZnQ6IDFyZW07DQp9";
            return StreamToString(Base64StringToStream(_result));
        }

        private static string AirCSS()
        {
            string _result = "QG1lZGlhIHByaW50IHsNCiAgKiwNCiAgKjpiZWZvcmUsDQogICo6YWZ0ZXIgew0KICAgIGJhY2tncm91bmQ6IHRyYW5zcGFyZW50ICFpbXBvcnRhbnQ7DQogICAgY29sb3I6ICMwMDAgIWltcG9ydGFudDsNCiAgICBib3gtc2hhZG93OiBub25lICFpbXBvcnRhbnQ7DQogICAgdGV4dC1zaGFkb3c6IG5vbmUgIWltcG9ydGFudDsNCiAgfQ0KDQogIGEsDQogIGE6dmlzaXRlZCB7DQogICAgdGV4dC1kZWNvcmF0aW9uOiB1bmRlcmxpbmU7DQogIH0NCg0KICBhW2hyZWZdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiICgiIGF0dHIoaHJlZikgIikiOw0KICB9DQoNCiAgYWJiclt0aXRsZV06YWZ0ZXIgew0KICAgIGNvbnRlbnQ6ICIgKCIgYXR0cih0aXRsZSkgIikiOw0KICB9DQoNCiAgYVtocmVmXj0iIyJdOmFmdGVyLA0KICBhW2hyZWZePSJqYXZhc2NyaXB0OiJdOmFmdGVyIHsNCiAgICBjb250ZW50OiAiIjsNCiAgfQ0KDQogIHByZSwNCiAgYmxvY2txdW90ZSB7DQogICAgYm9yZGVyOiAxcHggc29saWQgIzk5OTsNCiAgICBwYWdlLWJyZWFrLWluc2lkZTogYXZvaWQ7DQogIH0NCg0KICB0aGVhZCB7DQogICAgZGlzcGxheTogdGFibGUtaGVhZGVyLWdyb3VwOw0KICB9DQoNCiAgdHIsDQogIGltZyB7DQogICAgcGFnZS1icmVhay1pbnNpZGU6IGF2b2lkOw0KICB9DQoNCiAgaW1nIHsNCiAgICBtYXgtd2lkdGg6IDEwMCUgIWltcG9ydGFudDsNCiAgfQ0KDQogIHAsDQogIGgyLA0KICBoMyB7DQogICAgb3JwaGFuczogMzsNCiAgICB3aWRvd3M6IDM7DQogIH0NCg0KICBoMiwNCiAgaDMgew0KICAgIHBhZ2UtYnJlYWstYWZ0ZXI6IGF2b2lkOw0KICB9DQp9DQoNCmh0bWwgew0KICBmb250LXNpemU6IDEycHg7DQp9DQoNCkBtZWRpYSBzY3JlZW4gYW5kIChtaW4td2lkdGg6IDMycmVtKSBhbmQgKG1heC13aWR0aDogNDhyZW0pIHsNCiAgaHRtbCB7DQogICAgZm9udC1zaXplOiAxNXB4Ow0KICB9DQp9DQoNCkBtZWRpYSBzY3JlZW4gYW5kIChtaW4td2lkdGg6IDQ4cmVtKSB7DQogIGh0bWwgew0KICAgIGZvbnQtc2l6ZTogMTZweDsNCiAgfQ0KfQ0KDQpib2R5IHsNCiAgbGluZS1oZWlnaHQ6IDEuODU7DQp9DQoNCnAsDQouYWlyLXAgew0KICBmb250LXNpemU6IDFyZW07DQogIG1hcmdpbi1ib3R0b206IDEuM3JlbTsNCn0NCg0KaDEsDQouYWlyLWgxLA0KaDIsDQouYWlyLWgyLA0KaDMsDQouYWlyLWgzLA0KaDQsDQouYWlyLWg0IHsNCiAgbWFyZ2luOiAxLjQxNHJlbSAwIC41cmVtOw0KICBmb250LXdlaWdodDogaW5oZXJpdDsNCiAgbGluZS1oZWlnaHQ6IDEuNDI7DQp9DQoNCmgxLA0KLmFpci1oMSB7DQogIG1hcmdpbi10b3A6IDA7DQogIGZvbnQtc2l6ZTogMy45OThyZW07DQp9DQoNCmgyLA0KLmFpci1oMiB7DQogIGZvbnQtc2l6ZTogMi44MjdyZW07DQp9DQoNCmgzLA0KLmFpci1oMyB7DQogIGZvbnQtc2l6ZTogMS45OTlyZW07DQp9DQoNCmg0LA0KLmFpci1oNCB7DQogIGZvbnQtc2l6ZTogMS40MTRyZW07DQp9DQoNCmg1LA0KLmFpci1oNSB7DQogIGZvbnQtc2l6ZTogMS4xMjFyZW07DQp9DQoNCmg2LA0KLmFpci1oNiB7DQogIGZvbnQtc2l6ZTogLjg4cmVtOw0KfQ0KDQpzbWFsbCwNCi5haXItc21hbGwgew0KICBmb250LXNpemU6IC43MDdlbTsNCn0NCg0KLyogaHR0cHM6Ly9naXRodWIuY29tL21ybXJzL2ZsdWlkaXR5ICovDQoNCmltZywNCmNhbnZhcywNCmlmcmFtZSwNCnZpZGVvLA0Kc3ZnLA0Kc2VsZWN0LA0KdGV4dGFyZWEgew0KICBtYXgtd2lkdGg6IDEwMCU7DQp9DQoNCkBpbXBvcnQgdXJsKGh0dHA6Ly9mb250cy5nb29nbGVhcGlzLmNvbS9jc3M/ZmFtaWx5PU9wZW4rU2FuczozMDBpdGFsaWMsMzAwKTsNCg0KYm9keSB7DQogIGNvbG9yOiAjNDQ0Ow0KICBmb250LWZhbWlseTogJ09wZW4gU2FucycsIEhlbHZldGljYSwgc2Fucy1zZXJpZjsNCiAgZm9udC13ZWlnaHQ6IDMwMDsNCiAgbWFyZ2luOiA2cmVtIGF1dG8gMXJlbTsNCiAgbWF4LXdpZHRoOiA0OHJlbTsNCiAgdGV4dC1hbGlnbjogY2VudGVyOw0KfQ0KDQppbWcgew0KICBib3JkZXItcmFkaXVzOiA1MCU7DQogIGhlaWdodDogMjAwcHg7DQogIG1hcmdpbjogMCBhdXRvOw0KICB3aWR0aDogMjAwcHg7DQp9DQoNCmEsDQphOnZpc2l0ZWQgew0KICBjb2xvcjogIzM0OThkYjsNCn0NCg0KYTpob3ZlciwNCmE6Zm9jdXMsDQphOmFjdGl2ZSB7DQogIGNvbG9yOiAjMjk4MGI5Ow0KfQ0KDQpwcmUgew0KICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmFmYWZhOw0KICBwYWRkaW5nOiAxcmVtOw0KICB0ZXh0LWFsaWduOiBsZWZ0Ow0KfQ0KDQpibG9ja3F1b3RlIHsNCiAgbWFyZ2luOiAwOw0KICBib3JkZXItbGVmdDogNXB4IHNvbGlkICM3YTdhN2E7DQogIGZvbnQtc3R5bGU6IGl0YWxpYzsNCiAgcGFkZGluZzogMS4zM2VtOw0KICB0ZXh0LWFsaWduOiBsZWZ0Ow0KfQ0KDQp1bCwNCm9sLA0KbGkgew0KICB0ZXh0LWFsaWduOiBsZWZ0Ow0KfQ0KDQpwIHsNCiAgY29sb3I6ICM3Nzc7DQp9";
            return StreamToString(Base64StringToStream(_result));
        }
    }
}