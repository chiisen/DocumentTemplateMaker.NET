--- 更新注單語法
UPDATE wagers_1.wagers_bet SET JPConGoldOriginal = (RealBetGold * 0.0052) WHERE Adddate >= "2021-#Month#-#Day# 00:00:00" AND Adddate <= "2021-#Month#-#Day# 23:59:59" AND GGID != 1 AND IsDemo = 0;
