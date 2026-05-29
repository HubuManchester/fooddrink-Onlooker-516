using FoodPicker.Models;

namespace FoodPicker.Services;

/// 提供美食数据，包括随机抽取逻辑
public class FoodDataService
{
    private readonly List<FoodItem> _foods;
    private readonly Random _random = new();

    public FoodDataService()
    {
        _foods = new List<FoodItem>
        {
            // ===== 川渝美食 (7种) =====
            new() { Name = "火锅",        Emoji = "🍲", Category = "川渝美食", Description = "麻辣鲜香的四川火锅，牛油锅底配上毛肚、黄喉、鹅肠，越煮越入味。", Pairing = "🧊 冰粉 — 解辣必备，吃完火锅来一碗，透心凉" },
            new() { Name = "酸菜鱼",      Emoji = "🐟", Category = "川渝美食", Description = "酸辣开胃的重庆名菜，嫩滑鱼片配上老坛酸菜，鲜香四溢。", Pairing = "🍚 白米饭 — 酸菜鱼汤泡饭，人间极品" },
            new() { Name = "麻辣烫",      Emoji = "🥘", Category = "川渝美食", Description = "想吃啥就煮啥，麻辣汤底里涮各种食材，方便又过瘾的街头美食。", Pairing = "🧃 酸梅汤 — 解辣又消食，麻辣好搭档" },
            new() { Name = "水煮牛肉",    Emoji = "🥩", Category = "川渝美食", Description = "川菜代表作，嫩滑的牛肉片在麻辣汤中烫熟，上面铺满花椒和干辣椒，香气扑鼻。", Pairing = "🧊 红糖冰粉 — 吃完麻辣来碗冰粉，嘴巴瞬间降温" },
            new() { Name = "担担面",      Emoji = "🍜", Category = "川渝美食", Description = "成都名小吃，肉末炒得酥香，拌上芝麻酱和红油辣椒，面条筋道入味。", Pairing = "🥟 钟水饺 — 一干一湿，成都人的面食双绝" },
            new() { Name = "毛血旺",      Emoji = "🥘", Category = "川渝美食", Description = "重庆江湖菜代表，鸭血、毛肚、午餐肉在麻辣红汤里翻滚，越吃越上瘾。", Pairing = "🍺 冰啤酒 — 辣到飞起，一口啤酒回魂" },
            new() { Name = "口水鸡",      Emoji = "🍗", Category = "川渝美食", Description = "白切鸡淋上红亮亮的辣椒油，撒上芝麻花生碎，看着就让人流口水。", Pairing = "🍚 白米饭 — 口水鸡的汤汁拌饭，绝了" },

            // ===== 西北风味 (7种) =====
            new() { Name = "肉夹馍",      Emoji = "🥙", Category = "西北风味", Description = "陕西著名小吃，酥脆的馍夹着软烂入味的腊汁肉，香而不腻。", Pairing = "🍽️ 凉皮 — 陕西经典三秦套餐" },
            new() { Name = "兰州拉面",    Emoji = "🍝", Category = "西北风味", Description = "手工拉制的面条筋道有嚼劲，牛肉清汤配上白萝卜片和香菜蒜苗。", Pairing = "🥚 卤蛋 — 加颗卤蛋，拉面的灵魂伴侣" },
            new() { Name = "凉皮",        Emoji = "🍽️", Category = "西北风味", Description = "陕西凉皮筋道爽滑，拌上辣椒油、蒜水、芝麻酱、醋，夏天吃特别开胃。", Pairing = "🥙 肉夹馍 — 西安人的快乐套餐" },
            new() { Name = "羊肉泡馍",    Emoji = "🍲", Category = "西北风味", Description = "西安名吃，把馍掰成小块泡进滚烫的羊肉汤里，配上糖蒜和辣酱，太香了。", Pairing = "🧄 糖蒜 — 解腻提味，泡馍的灵魂配菜" },
            new() { Name = "油泼面",      Emoji = "🍜", Category = "西北风味", Description = "陕西BiangBiang面，滚烫的热油浇在辣椒面和蒜末上，滋啦一声香气炸开。", Pairing = "🥒 拍黄瓜 — 清爽解腻，一口面一口瓜" },
            new() { Name = "大盘鸡",      Emoji = "🍗", Category = "西北风味", Description = "新疆硬菜，大块鸡肉和土豆炖得软烂入味，最后拌上宽面一起吃，超级满足。", Pairing = "🍺 乌苏啤酒 — 新疆人吃大盘鸡的标配" },
            new() { Name = "手抓羊肉",    Emoji = "🥩", Category = "西北风味", Description = "西北牧区传统吃法，羊肉清水煮好蘸椒盐吃，原汁原味的鲜美。", Pairing = "🧅 生洋葱 — 西北人吃手抓肉的经典搭配" },

            // ===== 粤式美食 (7种) =====
            new() { Name = "煲仔饭",      Emoji = "🍚", Category = "粤式美食", Description = "米饭在砂锅里煲到焦香，铺上腊肠和青菜，淋上酱油拌匀，锅巴是精华。", Pairing = "🍵 老火例汤 — 一饭一汤，广东人的日常标配" },
            new() { Name = "叉烧饭",      Emoji = "🍖", Category = "粤式美食", Description = "港式经典，蜜汁叉烧色泽红亮，肉质软嫩，配上白米饭和烫青菜，简单又美味。", Pairing = "🍋 咸柠七 — 港式茶餐厅标配" },
            new() { Name = "肠粉",        Emoji = "🥢", Category = "粤式美食", Description = "广东早餐之王，米浆蒸成薄皮卷上虾仁或牛肉，淋上甜酱油，滑嫩爽口。", Pairing = "🥣 皮蛋瘦肉粥 — 一粉一粥，广东人早茶灵魂" },
            new() { Name = "虾饺",        Emoji = "🦐", Category = "粤式美食", Description = "广式早茶四大天王之首，水晶皮晶莹剔透，里面包着整只大虾仁，一口爆汁。", Pairing = "🍵 普洱茶 — 吃虾饺配普洱，解腻消食" },
            new() { Name = "白切鸡",      Emoji = "🍗", Category = "粤式美食", Description = "广东人做鸡的最高境界，皮爽肉滑，蘸上姜葱油，简单却无比鲜美。", Pairing = "🫚 姜葱油 — 没有姜葱油的白切鸡没有灵魂" },
            new() { Name = "烧鹅",        Emoji = "🦆", Category = "粤式美食", Description = "深井烧鹅，外皮烤得红亮酥脆，鹅肉鲜嫩多汁，蘸酸梅酱酸甜解腻。", Pairing = "🫐 酸梅酱 — 烧鹅的灵魂蘸料" },
            new() { Name = "云吞面",      Emoji = "🍜", Category = "粤式美食", Description = "港式经典，鲜虾云吞配竹升面，汤底用大地鱼和猪骨熬制，鲜到掉眉毛。", Pairing = "🥬 蚝油生菜 — 清清爽爽刚刚好" },

            // ===== 沪式小吃 (5种) =====
            new() { Name = "生煎包",      Emoji = "🥟", Category = "沪式小吃", Description = "上海经典小吃，底部煎得金黄焦脆，咬一口肉汁四溅，千万小心烫嘴。", Pairing = "🫗 香醋 + 姜丝 — 蘸醋解腻，正宗上海吃法" },
            new() { Name = "小笼包",      Emoji = "🥟", Category = "沪式小吃", Description = "皮薄馅大汤汁足，轻轻咬开吸汤汁，再蘸醋一口吃掉，满足到飞起。", Pairing = "🫗 镇江香醋 — 小笼包不蘸醋等于白吃" },
            new() { Name = "葱油拌面",    Emoji = "🍝", Category = "沪式小吃", Description = "上海人最简单的幸福，炸得焦香的葱油浇在面条上，拌一拌就是人间美味。", Pairing = "🥟 锅贴 — 葱油面配锅贴，上海人的碳水快乐" },
            new() { Name = "蟹粉豆腐",    Emoji = "🦀", Category = "沪式小吃", Description = "大闸蟹拆出的蟹黄蟹肉和嫩豆腐一起烩，蟹香浓郁，入口即化。", Pairing = "🍵 龙井茶 — 茶香配蟹香，江南人的讲究" },
            new() { Name = "排骨年糕",    Emoji = "🍖", Category = "沪式小吃", Description = "上海人从小吃到大的味道，炸猪排配软糯年糕，淋上甜面酱，咸甜交织。", Pairing = "🥤 冰豆浆 — 甜咸搭配，上海人的下午茶" },

            // ===== 北京风味/北方小吃 (6种) =====
            new() { Name = "烤鸭",        Emoji = "🦆", Category = "北京风味", Description = "北京招牌菜，外皮酥脆肉质鲜嫩，用薄饼卷上甜面酱和葱丝黄瓜，一口满足。", Pairing = "🫓 荷叶饼 — 薄饼卷鸭肉，北京人的讲究吃法" },
            new() { Name = "煎饼果子",    Emoji = "🫓", Category = "北方小吃", Description = "天津传统早点，绿豆面摊成薄饼打上鸡蛋，抹甜面酱夹上薄脆，层次丰富。", Pairing = "🥛 热豆浆 — 天津人的早餐标配" },
            new() { Name = "炸酱面",      Emoji = "🍝", Category = "北京风味", Description = "老北京家常面，黄酱和甜面酱炒的肉丁浇在过水面上，码上黄瓜丝豆芽。", Pairing = "🧄 生蒜瓣 — 吃炸酱面不来瓣蒜，味道少一半" },
            new() { Name = "涮羊肉",      Emoji = "🥘", Category = "北京风味", Description = "铜锅炭火清汤涮羊肉，薄如纸的羊肉片在沸汤里涮几下，蘸麻酱韭菜花。", Pairing = "🍶 二锅头 — 涮羊肉就二锅头，老北京的冬天" },
            new() { Name = "卤煮火烧",    Emoji = "🥘", Category = "北京风味", Description = "老北京重口味小吃，猪肠猪肺豆腐泡在卤汤里煮得入味，配火烧吃。", Pairing = "🥤 北冰洋汽水 — 北京人的肥宅快乐水" },
            new() { Name = "糖葫芦",      Emoji = "🍬", Category = "北方小吃", Description = "红彤彤的山楂裹上晶莹的冰糖，咬一口酸甜脆，童年的味道。", Pairing = "🌰 糖炒栗子 — 冬天街头的标配零食" },

            // ===== 东北菜 (5种) =====
            new() { Name = "锅包肉",      Emoji = "🥩", Category = "东北菜", Description = "东北名菜，猪里脊裹上淀粉炸得外酥里嫩，浇上酸甜汁，一口嘎嘣脆。", Pairing = "🍚 东北大米饭 — 锅包肉是下饭神器" },
            new() { Name = "地三鲜",      Emoji = "🥬", Category = "东北菜", Description = "土豆茄子青椒三种最普通的食材，过油炸了再炒，朴实却让人停不下筷子。", Pairing = "🍚 白米饭 — 地三鲜的汤汁拌饭是灵魂" },
            new() { Name = "猪肉炖粉条",  Emoji = "🍲", Category = "东北菜", Description = "东北硬菜，五花肉炖得软烂，粉条吸满肉汤，一口下去暖到心里。", Pairing = "🥟 酸菜饺子 — 一锅一饺，东北人的团圆饭" },
            new() { Name = "小鸡炖蘑菇",  Emoji = "🍗", Category = "东北菜", Description = "东北人招待贵客的硬菜，笨鸡和榛蘑一起炖，汤浓肉烂，香味飘三条街。", Pairing = "🍜 粉条 — 小鸡炖蘑菇里的粉条比肉还好吃" },
            new() { Name = "铁锅炖大鹅",  Emoji = "🍲", Category = "东北菜", Description = "围着大铁锅吃炖大鹅，锅边贴着玉米饼子，鹅肉炖得脱骨，冬天就靠这一锅。", Pairing = "🫓 贴饼子 — 蘸着鹅肉汤吃的玉米饼，绝了" },

            // ===== 广西美食 (3种) =====
            new() { Name = "螺蛳粉",      Emoji = "🍜", Category = "广西美食", Description = "柳州特色米粉，酸笋的独特风味配上螺蛳鲜汤，闻着臭吃着香，越嗦越上头。", Pairing = "🥛 冰豆奶 — 解辣解腻，柳州人的标配" },
            new() { Name = "桂林米粉",    Emoji = "🍜", Category = "广西美食", Description = "桂林人的早餐灵魂，卤水配锅烧牛肉，米粉爽滑配上酸豆角花生，一碗不够。", Pairing = "🥤 漓泉啤酒 — 桂林山水配桂林米粉" },
            new() { Name = "老友粉",      Emoji = "🍲", Category = "广西美食", Description = "南宁招牌，酸笋豆豉辣椒爆炒后煮粉，酸辣鲜香，南宁人说能治感冒。", Pairing = "🧊 龟苓膏 — 吃完辣来碗清凉下火" },

            // ===== 云南美食 (3种) =====
            new() { Name = "过桥米线",    Emoji = "🍲", Category = "云南美食", Description = "云南特色，滚烫的鸡汤里依次放入米线和各种配菜，汤鲜料足，仪式感满满。", Pairing = "🍗 汽锅鸡 — 云南两大名菜，一汤一粉" },
            new() { Name = "汽锅鸡",      Emoji = "🍗", Category = "云南美食", Description = "用建水紫陶汽锅蒸出来的鸡，一滴水不加，全靠蒸汽凝成汤，原汁原味。", Pairing = "🍄 野生菌 — 云南人吃鸡离不开菌子" },
            new() { Name = "鲜花饼",      Emoji = "🌸", Category = "云南美食", Description = "云南特色点心，酥皮里包着食用玫瑰花做的馅，咬一口花香四溢。", Pairing = "🍵 普洱茶 — 鲜花饼配普洱，云南人的下午茶" },

            // ===== 街头小吃 (5种) =====
            new() { Name = "烤串",        Emoji = "🍢", Category = "街头小吃", Description = "各种肉串在炭火上烤得滋滋冒油，撒上孜然和辣椒面，烟火气十足。", Pairing = "🍺 冰镇啤酒 — 烤串配啤酒，夏夜的最佳组合" },
            new() { Name = "臭豆腐",      Emoji = "🧈", Category = "街头小吃", Description = "闻着臭吃着香，外酥里嫩，浇上蒜汁辣椒和香菜，街边摊的灵魂美食。", Pairing = "🧋 珍珠奶茶 — 臭豆腐配奶茶，年轻人的奇怪搭配" },
            new() { Name = "烤冷面",      Emoji = "🍝", Category = "街头小吃", Description = "东北街头霸主，铁板上煎冷面，打鸡蛋刷酱料，卷起来切成段，便宜又好吃。", Pairing = "🥤 冰红茶 — 烤冷面的标配饮料" },
            new() { Name = "鸡蛋仔",      Emoji = "🥚", Category = "街头小吃", Description = "香港街头经典，外脆里软的鸡蛋仔，一个个小球掰着吃，奶香浓郁。", Pairing = "🍦 冰淇淋 — 鸡蛋仔夹冰淇淋，冰火两重天" },
            new() { Name = "铁板鱿鱼",    Emoji = "🦑", Category = "街头小吃", Description = "整只鱿鱼在铁板上压得滋滋响，刷上秘制酱料，香气飘半条街。", Pairing = "🧃 椰汁 — 解辣解腻刚刚好" },

            // ===== 其他各地美食 (5种) =====
            new() { Name = "热干面",      Emoji = "🍜", Category = "湖北美食", Description = "武汉人的早餐信仰，芝麻酱裹满每根面条，配上萝卜丁和葱花，拌开了趁热吃。", Pairing = "🥚 蛋酒 — 一碗热干面一碗蛋酒，武汉过早标配" },
            new() { Name = "沙茶面",      Emoji = "🍜", Category = "闽南美食", Description = "厦门特色，用沙茶酱熬的浓汤煮面，加上各种海鲜配料，汤头浓郁鲜香。", Pairing = "🦪 海蛎煎 — 厦门双绝，配着吃才过瘾" },
            new() { Name = "佛跳墙",      Emoji = "🍲", Category = "闽南美食", Description = "闽菜之王，鲍参翅肚加上花菇鸽蛋，用高汤文火慢炖，开坛飘香佛都跳墙。", Pairing = "🍚 白米饭 — 佛跳墙的汤拌饭，奢侈的享受" },
            new() { Name = "臭鳜鱼",      Emoji = "🐟", Category = "安徽美食", Description = "徽菜代表，鳜鱼经过发酵产生独特风味，闻着臭吃着香，鱼肉像蒜瓣一样嫩。", Pairing = "🍚 米饭 — 臭鳜鱼的汤汁是米饭杀手" },
            new() { Name = "牛肉面",      Emoji = "🍜", Category = "台湾美食", Description = "台湾招牌美食，红烧牛肉炖得入口即化，汤头浓郁，面条筋道，一碗暖到心。", Pairing = "🧋 珍珠奶茶 — 台湾两大名片，吃完面来杯奶茶" },
        };
    }

    public FoodItem GetRandomFood()
    {
        var index = _random.Next(_foods.Count);
        return _foods[index];
    }

    /// 根据关键词搜索食物，多层匹配策略
    public FoodItem? SearchFood(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return null;

        keyword = keyword.Trim();

        // 第1层：名称精确包含关键词（如搜"火锅"命中"火锅"）
        var match = _foods.FirstOrDefault(f => f.Name.Contains(keyword));
        if (match != null) return match;

        // 第2层：描述中包含关键词（如搜"麻辣"命中麻辣烫的描述）
        match = _foods.FirstOrDefault(f => f.Description.Contains(keyword));
        if (match != null) return match;

        // 第3层：分类包含关键词（如搜"川渝"命中川渝美食类）
        match = _foods.FirstOrDefault(f => f.Category.Contains(keyword));
        if (match != null) return match;

        // 第4层：逐字匹配 —— 关键词的每个字都在名称中出现
        // 如搜"面条" → "兰州拉面"不含"条"，但"炸酱面"也不含"条"
        // 降级为：名称至少包含关键词的任意一个字
        match = _foods.FirstOrDefault(f =>
            keyword.Any(ch => f.Name.Contains(ch)));
        if (match != null) return match;

        // 第5层：描述中包含关键词的任意一个字
        match = _foods.FirstOrDefault(f =>
            keyword.Any(ch => f.Description.Contains(ch)));
        return match;
    }

    public FoodItem GetRandomFoodExcluding(FoodItem? exclude)
    {
        FoodItem result;
        do
        {
            result = GetRandomFood();
        } while (exclude != null && result.Name == exclude.Name && _foods.Count > 1);
        return result;
    }
}
