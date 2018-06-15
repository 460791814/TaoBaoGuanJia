﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Helper
{
   public class ConfigHelper
    {
        /// <summary>
        /// 淘宝CSV头
        /// </summary>
        public static string[] TaoBaoHeaderRow = new string[]
                    {
                        "宝贝名称",
                        "宝贝类目",
                        "店铺类目",
                        "新旧程度",
                        "省",
                        "城市",
                        "出售方式",
                        "宝贝价格",
                        "加价幅度",
                        "宝贝数量",
                        "有效期",
                        "运费承担",
                        "平邮",
                        "EMS",
                        "快递",
                        "发票",
                        "保修",
                        "放入仓库",
                        "橱窗推荐",
                        "开始时间",
                        "宝贝描述",
                        "宝贝属性",
                        "邮费模版ID",
                        "会员打折",
                        "修改时间",
                        "上传状态",
                        "图片状态",
                        "返点比例",
                        "新图片",
                        "视频",
                        "销售属性组合",
                        "用户输入ID串",
                        "用户输入名-值对",
                        "商家编码",
                        "销售属性别名",
                        "代充类型",
                        "数字ID",
                        "本地ID",
                        "宝贝分类",
                        "账户名称",
                        "宝贝状态",
                        "闪电发货",
                        "新品",
                        "食品专项",
                        "库存计数",
                        "物流体积",
                        "物流重量",
                        "采购地",
                        "库存类型",
                        "国家地区",
                        "无线详情",
                        "商品条形码",
                        "宝贝卖点",
                        "sku 条形码",
                        "属性值备注",
                        "自定义属性值",
                        "尺码库",
                        "采购地",
                        "退换货承诺",
                        "定制工具",
                        "7天退货",
                        "商品资质",
                        "增加商品资质",
                        "关联线下服务"
                    };
        /// <summary>
        /// 淘宝CSV头字段
        /// </summary>
        public static string[] TaoBaoHeaderFieldRow = new string[]
            {
                            "title",
                            "cid",
                            "seller_cids",
                            "stuff_status",
                            "location_state",
                            "location_city",
                            "item_type",
                            "price",
                            "auction_increment",
                            "num",
                            "valid_thru",
                            "freight_payer",
                            "post_fee",
                            "ems_fee",
                            "express_fee",
                            "has_invoice",
                            "has_warranty",
                            "approve_status",
                            "has_showcase",
                            "list_time",
                            "description",
                            "cateProps",
                            "postage_id",
                            "has_discount",
                            "modified",
                            "upload_fail_msg",
                            "picture_status",
                            "auction_point",
                            "picture",
                            "video",
                            "skuProps",
                            "inputPids",
                            "inputValues",
                            "outer_id",
                            "propAlias",
                            "auto_fill",
                            "num_id",
                            "local_cid",
                            "navigation_type",
                            "user_name",
                            "syncStatus",
                            "is_lighting_consigment",
                            "is_xinpin",
                            "foodparame",
                            "sub_stock_type",
                            "item_size",
                            "item_weight",
                            "buyareatype",
                            "global_stock_type",
                            "global_stock_country",
                            "wireless_desc",
                            "barcode",
                            "subtitle",
                            "sku_barcode",
                            "cpv_memo",
                            "input_custom_cpv",
                            "features",
                            "buyareatype",
                            "sell_promise",
                            "custom_design_flag",
                            "newprepay",
                            "qualification",
                            "add_qualification",
                            "o2o_bind_service"
            };
        public static string TaoBaoCustomSellProperty = "床上用品>>床品套件/四件套/多件套,适用床尺寸|居家布艺>>缝纫DIY材料、工具及成品>>缝纫DIY配件/辅料/配饰>>纽扣,尺寸|服饰配件/皮带/帽子/围巾>>腰带/皮带/腰链,尺码|ZIPPO/瑞士军刀/眼镜>>替烟产品>>电子烟/烟油,烟油浓度|收纳整理>>家庭收纳用具>>收纳桶>>脏衣桶,尺寸|居家布艺>>窗帘门帘及配件>>门帘,尺寸|玩具/童车/益智/积木/模型>>儿童玩具枪>>电动玩具枪,套餐类型|居家布艺>>坐垫/椅垫/沙发垫>>椅垫,尺寸|收纳整理>>家庭收纳用具>>收纳盒>>鞋盒,尺寸|收纳整理>>家庭收纳用具>>收纳柜>>鞋柜,层数|收纳整理>>家庭收纳用具>>储物箱>>收纳箱,规格|居家布艺>>餐桌布艺>>椅套,尺寸|箱包皮具/热销女包/男包>>旅行箱,尺寸|收纳整理>>家庭防尘用具>>被子防尘袋,尺寸|收纳整理>>家庭防尘用具>>大衣/西服罩,尺寸|收纳整理>>家庭收纳用具>>收纳凳,容量|收纳整理>>家庭收纳用具>>收纳瓶,容量|收纳整理>>家庭收纳用具>>收纳袋>>收纳挂袋,尺寸|居家布艺>>靠垫/抱枕,尺寸|居家布艺>>地毯,地毯尺寸|居家布艺>>地垫,尺寸|居家布艺>>餐桌布艺>>餐垫,规格|居家布艺>>餐桌布艺>>桌布/桌旗/桌椅套/椅垫,具体规格|居家布艺>>餐桌布艺>>桌旗,规格|居家布艺>>餐桌布艺>>桌布,规格|居家布艺>>餐桌布艺>>餐巾,规格|居家布艺>>窗帘门帘及配件>>成品窗帘,窗帘尺寸+加工方式|居家布艺>>窗帘门帘及配件>>定制窗帘,尺寸|居家布艺>>防尘保护罩>>开关套,具体规格|居家布艺>>防尘保护罩>>万能盖巾,具体规格|居家布艺>>防尘保护罩>>钢琴罩,具体规格|居家布艺>>防尘保护罩>>饮水机罩,具体规格|居家布艺>>防尘保护罩>>微波炉罩,具体规格|居家布艺>>防尘保护罩>>洗衣机罩,具体规格|居家布艺>>防尘保护罩>>电视机罩,适用电视规格|居家布艺>>防尘保护罩>>空调罩,具体规格|居家布艺>>防尘保护罩>>沙发套,尺寸|居家布艺>>坐垫/椅垫/沙发垫>>沙发垫,规格|居家布艺>>坐垫/椅垫/沙发垫>>美臀垫/保健坐垫,尺寸|居家布艺>>坐垫/椅垫/沙发垫>>蒲团,尺寸|居家布艺>>坐垫/椅垫/沙发垫>>飘窗垫,尺寸|尿片/洗护/喂哺/推车床>>奶嘴/奶嘴相关>>奶嘴/安抚奶嘴,规格|尿片/洗护/喂哺/推车床>>防撞/提醒/安全/保护>>防撞条,长度|尿片/洗护/喂哺/推车床>>理发/指甲钳/量温等护理小用品>>宝宝剪刀/指甲钳,包装种类|尿片/洗护/喂哺/推车床>>睡袋/凉席/枕头/床品>>床褥,尺寸|尿片/洗护/喂哺/推车床>>睡袋/凉席/枕头/床品>>婴童床品套件,尺寸|尿片/洗护/喂哺/推车床>>睡袋/凉席/枕头/床品>>婴童床垫,尺寸|节庆用品/礼品>>喜字/剪纸/贴纸,尺寸|童装/婴儿装/亲子装>>裙子(新)>>连衣裙,参考身高|童装/婴儿装/亲子装>>裙子(新)>>半身裙,参考身高|童装/婴儿装/亲子装>>婴儿礼盒,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居服套装,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居服上装,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居裤/睡裤,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居裙/睡裙,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居袍/睡袍,参考身高|童装/婴儿装/亲子装>>儿童家居服>>家居服连体衣,参考身高|童装/婴儿装/亲子装>>儿童家居服>>浴袍,参考身高|童装/婴儿装/亲子装>>连身衣/爬服/哈衣,参考身高|童装/婴儿装/亲子装>>披风/斗篷,参考身高|童装/婴儿装/亲子装>>马甲,参考身高|童装/婴儿装/亲子装>>裤子,参考身高|童装/婴儿装/亲子装>>T恤,参考身高|童装/婴儿装/亲子装>>卫衣/绒衫,参考身高|童装/婴儿装/亲子装>>毛衣/针织衫,参考身高|童装/婴儿装/亲子装>>外套/夹克/大衣>>普通外套,参考身高|童装/婴儿装/亲子装>>外套/夹克/大衣>>西服/小西装,参考身高|童装/婴儿装/亲子装>>外套/夹克/大衣>>夹克/皮衣,参考身高|童装/婴儿装/亲子装>>外套/夹克/大衣>>呢大衣,参考身高|童装/婴儿装/亲子装>>外套/夹克/大衣>>风衣,参考身高|童装/婴儿装/亲子装>>棉袄/棉服,参考身高|童装/婴儿装/亲子装>>套装,参考身高|童装/婴儿装/亲子装>>亲子装/亲子时装,尺码|童装/婴儿装/亲子装>>校服/校服定制,参考身高|童装/婴儿装/亲子装>>儿童袜子(0-16岁),适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>帽子,适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>围巾,适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>手套,适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>多件套 帽子、围巾、手套等组合,适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>防抓手套/护脚/护膝,适用年龄|童装/婴儿装/亲子装>>帽子/围巾/口罩/手套/耳套/脚套>>口罩,适用年龄|童装/婴儿装/亲子装>>其它,适用年龄|童装/婴儿装/亲子装>>儿童旗袍/唐装/民族服装>>旗袍,参考身高|童装/婴儿装/亲子装>>儿童旗袍/唐装/民族服装>>唐装,参考身高|童装/婴儿装/亲子装>>反穿衣/罩衣,参考身高|童装/婴儿装/亲子装>>肚兜/肚围/护脐带>>肚兜,参考身高|童装/婴儿装/亲子装>>肚兜/肚围/护脐带>>肚围/护脐带,参考身高|童装/婴儿装/亲子装>>儿童内衣裤>>保暖上装,参考身高|童装/婴儿装/亲子装>>儿童内衣裤>>保暖裤,参考身高|童装/婴儿装/亲子装>>儿童内衣裤>>内裤,参考身高|童装/婴儿装/亲子装>>儿童内衣裤>>内衣套装,参考身高|童装/婴儿装/亲子装>>儿童泳装>>泳帽,参考身高|童装/婴儿装/亲子装>>儿童泳装>>泳衣裤,参考身高|童装/婴儿装/亲子装>>背心吊带,参考身高|童装/婴儿装/亲子装>>抹胸,参考身高|童装/婴儿装/亲子装>>儿童演出服,参考身高|童装/婴儿装/亲子装>>儿童礼服,参考身高|宠物/宠物食品及用品>>猫/狗日用品>>家用牵引带,适用尺码|";
        public static string TaoBaoCustomSellProperty2 = "节庆用品/礼品>>糖盒/糖盒配件,尺寸|节庆用品/礼品>>红包/利是封,具体规格|运动包/户外包/配件>>单肩背包,容量|运动包/户外包/配件>>运动鼓包/旅行包,容量|运动包/户外包/配件>>双肩背包,容量|洗护清洁剂/卫生巾/纸/香薰>>香熏用品>>香熏香料,香味|个性定制/设计服务/DIY>>日用/装饰定制>>竹简定制,片数|个性定制/设计服务/DIY>>日用/装饰定制>>相册/照片书/立体照片,尺寸&页数|新车/二手车>>整车特卖（专用）,车辆版本|新车/二手车>>新车定金,变速箱类型|新车/二手车>>新车全款,变速箱类型|成人用品/情趣用品>>男用器具>>阴臀倒模,规格类型|鲜花速递/花卉仿真/绿植园艺>>花瓶/花器/花盆/花架（新）>>花盆,尺寸|玩具/童车/益智/积木/模型>>音乐玩具/儿童乐器>>乐器套装,件数|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控轨道,套餐类型|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控机器人,套餐类型|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控船类,套餐类型|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控飞机,套餐类型|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控动物/人物,套餐类型|玩具/童车/益智/积木/模型>>电动/遥控/惯性/发条玩具>>电动/遥控车,套餐类型|玩具/童车/益智/积木/模型>>电动遥控玩具零件/工具>>遥控周边/设备,技术参数|玩具/童车/益智/积木/模型>>电动遥控玩具零件/工具>>遥控飞机零配件,飞机种类|玩具/童车/益智/积木/模型>>娃娃/配件,高度|运动/瑜伽/健身/球迷用品>>运动类活动/赛事,活动开始时间|床上用品>>被子>>棉花被,尺寸|床上用品>>被子>>化纤被,尺寸|床上用品>>被子>>蚕丝被,尺寸|床上用品>>被子>>羊毛被/驼毛被,尺寸|床上用品>>被子>>羽绒/羽毛被,尺寸|床上用品>>床盖,床单/床笠尺寸|床上用品>>床垫/床褥/床护垫/榻榻米床垫,适用床尺寸|床上用品>>被套,尺寸|床上用品>>床裙,床单/床笠尺寸|床上用品>>床笠,床单/床笠尺寸|床上用品>>床罩,床单/床笠尺寸|床上用品>>休闲毯/毛毯/绒毯,尺寸|厨房/烹饪用具>>厨用小工具/厨房储物>>厨房小工具套装,件数|厨房/烹饪用具>>烹饪用具>>烹饪工具套装,件数|居家日用>>毛巾/浴巾/浴袍>>手巾/手帕,尺寸|居家日用>>毛巾/浴巾/浴袍>>浴巾,尺寸|居家日用>>毛巾/浴巾/浴袍>>毛巾/面巾,尺寸|居家日用>>驱虫用品>>纱窗/纱门,尺寸|居家日用>>鞋用品>>鞋套机,鞋套容量|居家日用>>伞/雨具/防雨/防潮>>防潮垫/抽屉垫,尺寸|电子词典/电纸书/文化用品>>电子辞典,套餐|电子词典/电纸书/文化用品>>学习机,套餐|电子词典/电纸书/文化用品>>点读笔,套餐类型|平板电脑/MID,网络类型&存储容量&套餐类型|MP3/MP4/iPod/录音笔,套餐类型|商业/办公家具>>超市家具>>收银台,尺寸|家居饰品>>装饰画>>现代装饰画,组合形式&尺寸&外框类型|运动/瑜伽/健身/球迷用品>>踏步机/中小型健身器材>>哑铃,重量|宠物/宠物食品及用品>>猫/狗日用品>>窝/屋/帐篷,适用尺码|宠物/宠物食品及用品>>猫/狗日用品>>笼子,适用尺码|宠物/宠物食品及用品>>猫/狗日用品>>背包/箱包,适用尺码|宠物/宠物食品及用品>>猫/狗日用品>>航空箱,适用尺码|宠物/宠物食品及用品>>猫/狗日用品>>项圈,适用尺码|宠物/宠物食品及用品>>猫/狗日用品>>嘴套,适用尺码|家居饰品>>装饰画>>国画,组合形式&尺寸|商业/办公家具>>办公家具>>办公柜类>>文件柜,厚度|家装主材>>地板>>实木地板,计价单位|尿片/洗护/喂哺/推车床>>睡袋/凉席/枕头/床品>>睡袋/防踢被,长度|宠物/宠物食品及用品>>猫/狗日用品>>家用牵引带,适用尺码|智能设备>>智能手表,表壳尺寸&表带款式&系列|家庭/个人清洁工具>>卫浴/置物用具>>地垫,尺寸|玩具/童车/益智/积木/模型>>电子/发光/充气/整蛊玩具>>创意/整蛊玩具,大小描述|3C数码配件>>USB电脑周边>>USB HUB/转换器,长度|住宅家具>>沙发类>>布艺沙发,几人坐|家装主材>>地板>>实木复合地板,计价单位|电脑硬件/显示器/电脑周边>>电脑周边>>鼠标垫/贴/腕垫,尺寸&厚度|";
        public static string GetCurrentDomainDirectory()
        {
            return System.Environment.CurrentDirectory;
        }

        public static bool TaoBaoCItemPicShieldCheck = false;
        public static bool TaoBaoSellProperyPic = false;
        public static bool TaoBaoSnatchPromoPrice = false;
        public static bool TaoBaoSnatchSellPoint = false;
        public static bool TaoBaoFirstMianPic = false;

        public static bool IsExportMobileDesc = false;
        /// <summary>
        /// 获取CSV保存目录
        /// </summary>
        /// <returns></returns>
        public static string GetCsvPath() {
            return System.Environment.CurrentDirectory+"/csv/";
        }
        /// <summary>
        /// 同一个图片是否重复下载
        /// </summary>
        public static bool SamePicDownloadOneTime = false;

        public static string ExportDescSize = "2M";
    }
}
