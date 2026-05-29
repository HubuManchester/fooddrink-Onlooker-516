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
            new() { Name = "火锅",        Emoji = "🍲", Category = "川渝美食", Description = "麻辣鲜香的四川火锅，牛油锅底配上毛肚、黄喉、鹅肠，越煮越入味。", Pairing = "🧊 冰粉 — 解辣必备，吃完火锅来一碗，透心凉" },
            new() { Name = "酸菜鱼",      Emoji = "🐟", Category = "川渝美食", Description = "酸辣开胃的重庆名菜，嫩滑鱼片配上老坛酸菜，鲜香四溢。", Pairing = "🍚 白米饭 — 酸菜鱼汤泡饭，人间极品" },
            new() { Name = "肉夹馍",      Emoji = "🥙", Category = "西北风味", Description = "陕西著名小吃，酥脆的馍夹着软烂入味的腊汁肉，香而不腻。", Pairing = "🍽️ 凉皮 — 陕西经典三秦套餐，馍夹肉配凉皮" },
            new() { Name = "螺蛳粉",      Emoji = "🍜", Category = "广西美食", Description = "柳州特色米粉，酸笋的独特风味配上螺蛳鲜汤，闻着臭吃着香。", Pairing = "🥛 冰豆奶 — 解辣解腻，柳州人的标配" },
            new() { Name = "烤鸭",        Emoji = "🦆", Category = "北京风味", Description = "北京招牌菜，外皮酥脆肉质鲜嫩，用薄饼卷上甜面酱和葱丝黄瓜，一口满足。", Pairing = "🫓 荷叶饼 — 薄饼卷鸭肉，北京人的讲究吃法" },
            new() { Name = "煲仔饭",      Emoji = "🍚", Category = "粤式美食", Description = "广东经典，米饭在砂锅里煲到焦香，铺上腊肠和青菜，淋上酱油拌匀。", Pairing = "🍵 老火例汤 — 一饭一汤，广东人的日常标配" },
            new() { Name = "兰州拉面",    Emoji = "🍝", Category = "西北风味", Description = "手工拉制的面条筋道有嚼劲，牛肉清汤配上白萝卜片和香菜蒜苗。", Pairing = "🥚 卤蛋 — 加颗卤蛋，拉面的灵魂伴侣" },
            new() { Name = "生煎包",      Emoji = "🥟", Category = "沪式小吃", Description = "上海经典小吃，底部煎得金黄焦脆，咬一口肉汁四溢，外酥内嫩。", Pairing = "🫗 香醋 + 姜丝 — 蘸醋解腻，正宗上海吃法" },
            new() { Name = "麻辣烫",      Emoji = "🥘", Category = "川渝美食", Description = "想吃啥就煮啥，麻辣汤底里涮各种食材，方便又过瘾的街头美食。", Pairing = "🧃 酸梅汤 — 解辣又消食，麻辣好搭档" },
            new() { Name = "凉皮",        Emoji = "🍽️", Category = "西北风味", Description = "陕西凉皮筋道爽滑，拌上辣椒油、蒜水、芝麻酱、醋，夏天吃特别开胃。", Pairing = "🥙 肉夹馍 — 凉皮配肉夹馍，西安人的快乐套餐" },
            new() { Name = "叉烧饭",      Emoji = "🍖", Category = "粤式美食", Description = "港式经典，蜜汁叉烧色泽红亮，肉质软嫩，配上白米饭和烫青菜，简单又美味。", Pairing = "🍋 咸柠七 — 港式茶餐厅标配，咸柠七配叉烧饭" },
            new() { Name = "水煮牛肉",    Emoji = "🥩", Category = "川渝美食", Description = "川菜代表作，嫩滑的牛肉片在麻辣汤中烫熟，上面铺满花椒和干辣椒，香气扑鼻。", Pairing = "🧊 红糖冰粉 — 吃完麻辣来碗冰粉，嘴巴瞬间降温" },
            new() { Name = "烤串",        Emoji = "🍢", Category = "街头小吃", Description = "各种肉串在炭火上烤得滋滋冒油，撒上孜然和辣椒面，烟火气十足。", Pairing = "🍺 冰镇啤酒 — 烤串配啤酒，夏夜的最佳组合" },
            new() { Name = "煎饼果子",    Emoji = "🫓", Category = "北方小吃", Description = "天津传统早点，绿豆面摊成薄饼，打上鸡蛋，抹甜面酱，夹上薄脆，层次感丰富。", Pairing = "🥛 热豆浆 — 天津人的早餐标配，煎饼配豆浆" },
            new() { Name = "过桥米线",    Emoji = "🍲", Category = "云南美食", Description = "云南特色，滚烫的鸡汤里依次放入米线和各种配菜，汤鲜料足，仪式感满满。", Pairing = "🍗 汽锅鸡 — 云南两大名菜，一汤一粉相得益彰" },
        };
    }

    public FoodItem GetRandomFood()
    {
        var index = _random.Next(_foods.Count);
        return _foods[index];
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
