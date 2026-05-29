using FoodPicker.Models;

namespace FoodPicker.Services;

/// Food data provider with random selection and search logic
public class FoodDataService
{
    private readonly List<FoodItem> _foods;
    private readonly Random _random = new();

    public FoodDataService()
    {
        _foods = new List<FoodItem>
        {
            // ===== Sichuan Cuisine (7) =====
            new() { Name = "Hotpot",        Emoji = "🍲", Category = "Sichuan Cuisine", Description = "Spicy Sichuan hotpot with beef tallow broth. Loaded with tripe, duck intestine and fresh vegetables that get richer the longer they simmer.", Pairing = "🧊 Iced Jelly — essential palate cooler after all that numbing heat" },
            new() { Name = "Pickled Fish",  Emoji = "🐟", Category = "Sichuan Cuisine", Description = "Tangy and spicy Chongqing classic. Tender fish fillets swimming in a broth of aged pickled mustard greens, aromatic and zesty.", Pairing = "🍚 Steamed Rice — fish broth over rice is pure heaven" },
            new() { Name = "Malatang",      Emoji = "🥘", Category = "Sichuan Cuisine", Description = "Build your own bowl: pick any ingredients and dunk them in bubbling spicy broth. The ultimate customizable street food experience.", Pairing = "🧃 Sour Plum Drink — cools the burn, classic combo" },
            new() { Name = "Boiled Beef",   Emoji = "🥩", Category = "Sichuan Cuisine", Description = "Tender beef slices poached in fiery chili oil, blanketed with Sichuan peppercorns and dried chilies. Aromatic and mouth-numbing.", Pairing = "🧊 Brown Sugar Jelly — instant cooldown after the spice" },
            new() { Name = "Dan Dan Noodles", Emoji = "🍜", Category = "Sichuan Cuisine", Description = "Chengdu street classic. Crispy minced pork over springy noodles, tossed in sesame paste and chili oil. Pure comfort in a bowl.", Pairing = "🥟 Zhong Dumplings — Chengdu's noodle-dumpling dream team" },
            new() { Name = "Blood Curd Stew", Emoji = "🥘", Category = "Sichuan Cuisine", Description = "Chongqing pub favourite. Duck blood, tripe and luncheon meat bubbling in a crimson spicy broth. Dangerously addictive.", Pairing = "🍺 Cold Beer — one sip after the heat and you are back in the game" },
            new() { Name = "Mouthwatering Chicken", Emoji = "🍗", Category = "Sichuan Cuisine", Description = "Poached chicken drenched in glossy red chili oil, sprinkled with sesame seeds and crushed peanuts. The name says it all.", Pairing = "🍚 Steamed Rice — that chili oil over rice is unbeatable" },

            // ===== Northwest Flavors (7) =====
            new() { Name = "Roujiamo",      Emoji = "🥙", Category = "Northwest Flavors", Description = "Shaanxi's famous meat burger. Crispy flatbread stuffed with slow-braised, melt-in-your-mouth pork belly. Simple and perfect.", Pairing = "🍽️ Liangpi — the classic Shaanxi food combo" },
            new() { Name = "Lanzhou Noodles", Emoji = "🍝", Category = "Northwest Flavors", Description = "Hand-pulled noodles with incredible chew, swimming in clear beef broth with radish slices and fresh cilantro. Five generations of craft.", Pairing = "🥚 Marinated Egg — the soulmate of every bowl of noodles" },
            new() { Name = "Liangpi",       Emoji = "🍽️", Category = "Northwest Flavors", Description = "Chewy cold skin noodles tossed in chili oil, garlic water, sesame paste and vinegar. A summer appetite savior from the streets of Xi'an.", Pairing = "🥙 Roujiamo — Xi'an's ultimate happiness combo" },
            new() { Name = "Lamb Paomo",    Emoji = "🍲", Category = "Northwest Flavors", Description = "Xi'an soul food. Tear flatbread into pieces yourself, then soak it in bubbling lamb broth with sweet garlic and chili paste.", Pairing = "🧄 Sweet Pickled Garlic — the essential paomo sidekick" },
            new() { Name = "Oil-Splashed Noodles", Emoji = "🍜", Category = "Northwest Flavors", Description = "Shaanxi Biangbiang noodles. Sizzling hot oil poured over chili flakes and minced garlic — that crackling sound is pure culinary joy.", Pairing = "🥒 Smashed Cucumber — light and crisp, the perfect counterbalance" },
            new() { Name = "Big Plate Chicken", Emoji = "🍗", Category = "Northwest Flavors", Description = "Xinjiang feast dish. Chunky chicken and potatoes braised until tender, then tossed with wide noodles to soak up every last drop of sauce.", Pairing = "🍺 Wusu Beer — the mandatory Xinjiang pairing" },
            new() { Name = "Hand-Pulled Lamb", Emoji = "🥩", Category = "Northwest Flavors", Description = "Pastoral tradition of the northwest. Lamb boiled simply and eaten by hand with spiced salt. Pure, primal, delicious.", Pairing = "🧅 Raw Onion — the classic shepherd's accompaniment" },

            // ===== Cantonese Cuisine (7) =====
            new() { Name = "Claypot Rice",  Emoji = "🍚", Category = "Cantonese Cuisine", Description = "Rice cooked in a clay pot until crispy on the bottom, topped with lap cheong sausage and greens. The scorched crust is the real prize.", Pairing = "🍵 Slow-fire Soup — one rice, one soup, the Cantonese daily ritual" },
            new() { Name = "Char Siu Rice", Emoji = "🍖", Category = "Cantonese Cuisine", Description = "Hong Kong classic. Glazed barbecue pork, glossy red and impossibly tender, over steamed rice with blanched greens. Simple but perfect.", Pairing = "🍋 Salty Lime Soda — quintessential cha chaan teng drink" },
            new() { Name = "Cheung Fun",    Emoji = "🥢", Category = "Cantonese Cuisine", Description = "The king of Cantonese breakfast. Silky rice rolls wrapped around plump shrimp or tender beef, drizzled with sweet soy sauce.", Pairing = "🥣 Century Egg Congee — one roll, one congee, dim sum soulmates" },
            new() { Name = "Har Gow",       Emoji = "🦐", Category = "Cantonese Cuisine", Description = "The crown jewel of dim sum. Translucent crystal skin encasing a whole succulent shrimp. One bite and the juice explodes in your mouth.", Pairing = "🍵 Pu'er Tea — cuts through the richness perfectly" },
            new() { Name = "White Cut Chicken", Emoji = "🍗", Category = "Cantonese Cuisine", Description = "Cantonese chicken at its purest. Silky skin, tender flesh, dipped in fragrant ginger-scallion oil. Deceptively simple, utterly sublime.", Pairing = "🫚 Ginger-Scallion Oil — white cut chicken is naked without it" },
            new() { Name = "Roast Goose",   Emoji = "🦆", Category = "Cantonese Cuisine", Description = "Deep-well roast goose. Glossy, crackling skin giving way to juicy, flavourful meat. Dip in sour plum sauce for sweet-tangy perfection.", Pairing = "🫐 Sour Plum Sauce — the soul of roast goose" },
            new() { Name = "Wonton Noodles", Emoji = "🍜", Category = "Cantonese Cuisine", Description = "Hong Kong institution. Plump shrimp wontons with bamboo-pressed noodles in a rich broth of dried flounder and pork bones. Umami bomb.", Pairing = "🥬 Oyster Sauce Lettuce — clean and crisp, just right" },

            // ===== Shanghai Delicacies (5) =====
            new() { Name = "Shengjian Bao", Emoji = "🥟", Category = "Shanghai Delicacies", Description = "Shanghai icon. Pan-fried buns with golden crispy bottoms, bursting with scalding-hot meaty soup. Bite carefully or you will regret it!", Pairing = "🫗 Black Vinegar + Ginger — authentic Shanghai dipping ritual" },
            new() { Name = "Xiaolongbao",   Emoji = "🥟", Category = "Shanghai Delicacies", Description = "Delicate soup dumplings with paper-thin skin. Nibble a hole, sip the broth, then dunk in vinegar and devour. Pure bliss in a steamer basket.", Pairing = "🫗 Zhenjiang Vinegar — eating xiaolongbao without vinegar is a crime" },
            new() { Name = "Scallion Noodles", Emoji = "🍝", Category = "Shanghai Delicacies", Description = "Shanghai's simplest joy. Crispy fried scallion oil tossed through springy noodles. Three ingredients, infinite satisfaction.", Pairing = "🥟 Potstickers — scallion noodles plus potstickers, Shanghai carb heaven" },
            new() { Name = "Crab Roe Tofu", Emoji = "🦀", Category = "Shanghai Delicacies", Description = "Hairy crab roe and meat simmered with silken tofu until rich and velvety. Luxurious and melts on the tongue. A Yangtze River Delta delicacy.", Pairing = "🍵 Longjing Tea — tea fragrance meets crab richness, Jiangnan elegance" },
            new() { Name = "Rib Rice Cake", Emoji = "🍖", Category = "Shanghai Delicacies", Description = "A taste of Shanghai childhood. Fried pork cutlet paired with chewy rice cakes, drizzled with sweet bean sauce. Sweet-salty magic on a plate.", Pairing = "🥤 Iced Soy Milk — sweet and salty, classic Shanghai afternoon tea" },

            // ===== Beijing Cuisine / Northern Snacks (6) =====
            new() { Name = "Peking Duck",   Emoji = "🦆", Category = "Beijing Cuisine", Description = "Beijing's crowning glory. Crispy lacquered skin and tender meat, wrapped in thin pancakes with sweet bean sauce, cucumber and scallion.", Pairing = "🫓 Lotus Leaf Pancake — the proper Beijing way to enjoy duck" },
            new() { Name = "Jianbing",      Emoji = "🫓", Category = "Northern Snacks", Description = "Tianjin's morning ritual. Mung bean crepe with egg, smeared with sweet bean paste and folded around a crispy cracker. Layers of pure texture.", Pairing = "🥛 Hot Soy Milk — Tianjin's classic breakfast standard" },
            new() { Name = "Zhajiang Noodles", Emoji = "🍝", Category = "Beijing Cuisine", Description = "Old Beijing comfort food. Thick fermented soybean and sweet bean sauce with diced pork over cool noodles, topped with julienned cucumber.", Pairing = "🧄 Raw Garlic Clove — eating zhajiang mian without garlic is only half the flavour" },
            new() { Name = "Instant-Boiled Lamb", Emoji = "🥘", Category = "Beijing Cuisine", Description = "Copper hotpot with charcoal fire. Paper-thin lamb slices swished in clear broth, then dipped in sesame paste with chive flower sauce.", Pairing = "🍶 Erguotou Baijiu — old Beijing winter, lamb and strong liquor" },
            new() { Name = "Luzhu Huoshao", Emoji = "🥘", Category = "Beijing Cuisine", Description = "Bold Beijing street eat. Pork intestines, lungs and tofu puffs simmered in spiced soy broth, served with wheat cakes. Not for the faint-hearted.", Pairing = "🥤 Arctic Ocean Soda — Beijing's own nostalgic soft drink" },
            new() { Name = "Candied Hawthorn", Emoji = "🍬", Category = "Northern Snacks", Description = "Glossy sugar-coated hawthorn berries on a bamboo stick. Sweet, sour, and satisfyingly crunchy. A taste of childhood winters in northern China.", Pairing = "🌰 Candied Chestnuts — winter's ultimate street snack duo" },

            // ===== Northeastern Cuisine (5) =====
            new() { Name = "Guo Bao Rou",   Emoji = "🥩", Category = "Northeastern Cuisine", Description = "Dongbei's signature dish. Pork loin coated in potato starch, double-fried to shattering crispness, then tossed in a tangy sweet-sour sauce.", Pairing = "🍚 Dongbei Rice — guo bao rou is the ultimate rice killer" },
            new() { Name = "Di San Xian",   Emoji = "🥬", Category = "Northeastern Cuisine", Description = "Three humble ingredients: potato, eggplant, green pepper. Flash-fried then wok-tossed together. Rustic, greasy, impossible to stop eating.", Pairing = "🍚 Steamed Rice — the savoury sauce over rice is everything" },
            new() { Name = "Pork Vermicelli Stew", Emoji = "🍲", Category = "Northeastern Cuisine", Description = "Hearty Dongbei stew. Fatty pork belly braised until spoon-tender, wide vermicelli soaking up the rich broth. Warms you from the inside out.", Pairing = "🥟 Sauerkraut Dumplings — one pot of stew, one plate of dumplings, the Dongbei family feast" },
            new() { Name = "Chicken Mushroom Stew", Emoji = "🍗", Category = "Northeastern Cuisine", Description = "The dish Dongbei hosts serve honoured guests. Free-range chicken and wild hazel mushrooms in a thick, fragrant broth that fills the whole house.", Pairing = "🍜 Vermicelli — the noodles in this stew somehow taste better than the chicken" },
            new() { Name = "Iron Pot Goose", Emoji = "🍲", Category = "Northeastern Cuisine", Description = "Gather round a giant iron pot of braised goose, with corn cakes baked right onto the sides. Goose so tender it falls off the bone. Winter survival food.", Pairing = "🫓 Corn Cakes — dipped in goose broth, absolutely divine" },

            // ===== Guangxi Cuisine (3) =====
            new() { Name = "Luosifen",      Emoji = "🍜", Category = "Guangxi Cuisine", Description = "Liuzhou's infamous river snail noodles. Fermented bamboo shoots deliver a pungent funk, but the rich snail broth keeps you coming back for more.", Pairing = "🥛 Iced Soy Milk — cools the burn, Liuzhou's standard chaser" },
            new() { Name = "Guilin Noodles", Emoji = "🍜", Category = "Guangxi Cuisine", Description = "Guilin's breakfast soul. Marinated beef and crispy pork over slick rice noodles, topped with pickled beans and roasted peanuts. One bowl is never enough.", Pairing = "🥤 Lijiang Beer — Guilin scenery deserves Guilin noodles" },
            new() { Name = "Laoyoufen",     Emoji = "🍲", Category = "Guangxi Cuisine", Description = "Nanning's claim to fame. Pickled bamboo shoots, black beans and chilies stir-fried then simmered with rice noodles. Locals swear it cures colds.", Pairing = "🧊 Turtle Jelly — a cooling herbal finish after the spice rush" },

            // ===== Yunnan Cuisine (3) =====
            new() { Name = "Crossing Bridge Noodles", Emoji = "🍲", Category = "Yunnan Cuisine", Description = "Yunnan's theatrical specialty. A scalding chicken broth served separately, into which you add noodles and toppings one by one. Ceremony in a bowl.", Pairing = "🍗 Steam Pot Chicken — Yunnan's two greatest dishes, soup and noodles united" },
            new() { Name = "Steam Pot Chicken", Emoji = "🍗", Category = "Yunnan Cuisine", Description = "Chicken steamed in a Jianshui clay pot. No water added — the broth forms purely from condensation. Pure, pristine, unadulterated essence of chicken.", Pairing = "🍄 Wild Mushrooms — Yunnan chicken always brings wild mushrooms to the party" },
            new() { Name = "Rose Pastry",   Emoji = "🌸", Category = "Yunnan Cuisine", Description = "Yunnan's edible souvenir. Flaky pastry filled with sweet rose petal jam. Bite in and the floral fragrance fills all your senses.", Pairing = "🍵 Pu'er Tea — rose pastry and pu'er, Yunnan's afternoon tea ritual" },

            // ===== Street Food (5) =====
            new() { Name = "BBQ Skewers",   Emoji = "🍢", Category = "Street Food", Description = "Every kind of meat on sticks, grilled over charcoal until sizzling, showered with cumin and chili flakes. Pure smoky, late-night joy.", Pairing = "🍺 Ice-Cold Beer — skewers and beer, the ultimate summer night combo" },
            new() { Name = "Stinky Tofu",   Emoji = "🧈", Category = "Street Food", Description = "Smells like trouble, tastes like heaven. Crispy outside, custardy inside, drenched in garlic-chili sauce with fresh cilantro. Street stall royalty.", Pairing = "🧋 Bubble Tea — stinky tofu plus bubble tea, the young generation's favourite odd couple" },
            new() { Name = "Grilled Cold Noodles", Emoji = "🍝", Category = "Street Food", Description = "Dongbei street king. A cold noodle sheet sizzled on a griddle, cracked with egg, brushed with savoury sauce, rolled up and sliced. Cheap and crazy good.", Pairing = "🥤 Iced Tea — the standard grilled cold noodle beverage of choice" },
            new() { Name = "Egg Waffles",   Emoji = "🥚", Category = "Street Food", Description = "Hong Kong street classic. Crispy outside, pillowy inside egg-shaped bubbles you pop off one by one. Milky, sweet, and dangerously moreish.", Pairing = "🍦 Ice Cream — egg waffle stuffed with soft serve, hot-cold heaven" },
            new() { Name = "Teppan Squid",  Emoji = "🦑", Category = "Street Food", Description = "A whole squid pressed sizzling onto a hot iron plate, brushed repeatedly with secret sauce. The aroma travels half a block. Irresistible.", Pairing = "🧃 Coconut Water — light and refreshing, the perfect palate cleanser" },

            // ===== Other Regional Cuisines (5) =====
            new() { Name = "Hot Dry Noodles", Emoji = "🍜", Category = "Hubei Cuisine", Description = "Wuhan's breakfast religion. Sesame paste coats every strand of noodle, topped with pickled radish and scallions. Mix well, eat hot, no regrets.", Pairing = "🥚 Egg Rice Wine — one bowl of noodles, one bowl of sweet egg wine, Wuhan morning ritual" },
            new() { Name = "Satay Noodles", Emoji = "🍜", Category = "Fujian Cuisine", Description = "Xiamen specialty. Rich, complex satay sauce broth with your choice of fresh seafood toppings and springy noodles. Deep, aromatic, unforgettable.", Pairing = "🦪 Oyster Omelette — Xiamen's dynamic duo, better together than apart" },
            new() { Name = "Buddha Jumps Wall", Emoji = "🍲", Category = "Fujian Cuisine", Description = "The undisputed king of Fujian cuisine. Abalone, sea cucumber, fish maw, flower mushrooms, and quail eggs all slow-braised in supreme broth.", Pairing = "🍚 Steamed Rice — that broth over rice is the very definition of luxury" },
            new() { Name = "Stinky Mandarin Fish", Emoji = "🐟", Category = "Anhui Cuisine", Description = "Anhui's fermented masterpiece. Mandarin fish develops a unique pungency through controlled fermentation. The flesh separates like garlic cloves. Bold and unforgettable.", Pairing = "🍚 Rice — the rich sauce is a certified rice annihilator" },
            new() { Name = "Beef Noodle Soup", Emoji = "🍜", Category = "Taiwanese Cuisine", Description = "Taiwan's calling card. Red-braised beef so tender it dissolves, in a deep, rich broth with springy hand-pulled noodles. A bowl of pure comfort.", Pairing = "🧋 Bubble Tea — Taiwan's two global ambassadors, noodles first then boba" },
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

        // Layer 1: exact name contains keyword
        var match = _foods.FirstOrDefault(f => f.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;

        // Layer 2: description contains keyword
        match = _foods.FirstOrDefault(f => f.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;

        // Layer 3: category contains keyword
        match = _foods.FirstOrDefault(f => f.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;

        // Layer 4: any character from keyword appears in the food name
        match = _foods.FirstOrDefault(f =>
            keyword.Any(ch => f.Name.Contains(ch, StringComparison.OrdinalIgnoreCase)));
        if (match != null) return match;

        // Layer 5: any character from keyword appears in the description
        match = _foods.FirstOrDefault(f =>
            keyword.Any(ch => f.Description.Contains(ch, StringComparison.OrdinalIgnoreCase)));
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
