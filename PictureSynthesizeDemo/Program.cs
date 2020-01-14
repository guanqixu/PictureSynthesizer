using PictureSynthesize;
using System.Drawing;

namespace PictureSynthesizerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PictureSynthesizer service = new PictureSynthesizer(1920, 1080, Color.FromArgb(255, 192, 0));

            string title = "广州世贸大厦";

            string description = @"项目位于广州市最繁华的商业黄金地带——广州环市东路。世贸大厦占地6900平方米，总建筑面积10万平方米，是由地下停车场、裙楼商场及两座30多层全玻璃墙塔组成的集商务办公、购物、餐饮、娱乐为一体的综合性甲级商业大厦。整座大厦设计精巧，外型高雅别致气势辉煌，南北两塔并立，犹如一部翻开的书卷，曾被评为广州现代十大名建筑之一；并获国家建筑设计最高奖——鲁班奖。

项目于1992年落成投入使用，1995年被评为全国城市物业管理优秀大厦；1997年底被评为广州市安全文明商厦。1999年1月，在全国同行业率先通过ISO9001国际质量认证。

项目因良好的硬件设施、便利的交通条件、优越的经营环境和优质高效的物业管理而受到众多中外商家的青睐，年平均出租率一直保持在90 % 以上。多家著名的跨国公司、国家级商贸机构在此设点办公，使世贸中心大厦成为名符其实的广州经贸界首选的甲级写字楼。";


            service.Add(title, new RectangleF(50, 50, 885, 100));

            service.Add(description, new RectangleF(50, 160, 885, 870));

            service.Add(@"..\..\..\batman.JPG", new RectangleF(985, 50, 885, 980));

            service.Synthesize("test.jpg");

        }
    }
}
