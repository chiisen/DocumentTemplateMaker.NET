SET @start_time := '2021-#StartMonth#-#StartDay# 00:00:00';
SET @end_time := '2021-#EndMonth#-#EndDay# 23:59:59';

 
DELETE FROM wagers_1.user_revenue_player WHERE AccountDate >= @start_time AND AccountDate < @end_time;
DELETE FROM wagers_1.user_revenue_agent WHERE AccountDate >= @start_time AND AccountDate < @end_time;
DELETE FROM wagers_1.user_revenue_hall WHERE AccountDate >= @start_time AND AccountDate < @end_time;
DELETE FROM wagers_1.game_revenue_player WHERE AccountDate >= @start_time AND AccountDate < @end_time;
DELETE FROM wagers_1.game_revenue_agent WHERE AccountDate >= @start_time AND AccountDate < @end_time;
DELETE FROM wagers_1.game_revenue_hall WHERE AccountDate >= @start_time AND AccountDate < @end_time;

-- user

INSERT INTO wagers_1.user_revenue_player
( Id,Rounds,Cid,UserName,UpId,HallId,CryDef,Currency,ExCurrency,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal ) 
SELECT Id,Rounds,Cid,UserName,UpId,HallId,CryDef,Currency,ExCurrency,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal  FROM 
( SELECT MD5(CONCAT_WS('-',Cid, left(AddDate,15),Currency,ExCurrency,CryDef)) AS Id,count(Cid) AS Rounds,Cid,UserName,UpId,HallId,CryDef,Currency,ExCurrency,IsDemo,SUM( TRUNCATE(BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE(BetPoint / 1000,2) *1000 ) AS BetPoint,SUM( TRUNCATE(WinGold / 1000,2) *1000 ) AS WinGold,SUM( TRUNCATE(JPPoint / 1000,2) *1000 ) AS JPPoint,SUM( TRUNCATE(JPGold / 1000,2) *1000 ) AS JPGold,CONCAT(left(AddDate,15),"0:00") AS AccountDate ,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal
FROM  wagers_1.wagers_bet   
WHERE AddDate >= @start_time AND AddDate < @end_time 
AND IsValid = 1 
AND IsDemo = 0 GROUP BY Cid,left(AddDate,15),
Currency,ExCurrency,CryDef,IsDemo ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold , UpId=tempTable.UpId, RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;

INSERT INTO wagers_1.user_revenue_agent( Id,Rounds,Cid,HallId,CryDef,Currency,ExCurrency,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal )
 SELECT Id,Rounds,Cid,HallId,CryDef,Currency,ExCurrency,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal 
 FROM ( SELECT MD5(CONCAT_WS('-',p.UpId,p.AccountDate,p.Currency,p.ExCurrency,p.CryDef)) AS Id,SUM(p.Rounds) AS Rounds,p.UpId AS Cid,p.HallId,p.CryDef,p.Currency,p.ExCurrency,p.IsDemo,SUM( TRUNCATE(p.BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE(p.BetPoint / 1000,2) *1000 ) AS BetPoint,SUM( TRUNCATE(p.WinGold / 1000,2) *1000 ) AS WinGold,SUM( TRUNCATE(p.JPPoint / 1000,2) *1000 ) AS JPPoint,SUM( TRUNCATE(p.JPGold / 1000,2) *1000 ) AS JPGold,p.AccountDate,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal
 FROM  wagers_1.user_revenue_player p  LEFT JOIN game.customer c ON c.Cid = p.UpId  
 WHERE p.AccountDate >= @start_time AND p.AccountDate < @end_time
 AND c.IsDemo = 0 GROUP BY p.UpId,p.AccountDate,p.Currency,p.ExCurrency,p.CryDef,c.IsDemo ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold , RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;
 
 INSERT INTO wagers_1.user_revenue_hall( Id,Rounds,Cid,CryDef,Currency,ExCurrency,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,Upid,RealBetGold,JPConGoldOriginal  ) 
 SELECT Id,Rounds,Cid,CryDef,Currency,ExCurrency,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,Upid,RealBetGold,JPConGoldOriginal 
 FROM ( SELECT MD5(CONCAT_WS('-',a.HallId, a.AccountDate,a.Currency,a.ExCurrency,a.CryDef)) AS Id,SUM(a.Rounds) AS Rounds,a.HallId AS Cid,a.CryDef,a.Currency,a.ExCurrency,SUM( TRUNCATE(a.BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE(a.BetPoint / 1000,2) * 1000 ) AS BetPoint,SUM( TRUNCATE(a.WinGold / 1000,2) * 1000 ) AS WinGold,SUM( TRUNCATE(a.JPPoint / 1000,2) * 1000 ) AS JPPoint,SUM( TRUNCATE(a.JPGold / 1000,2) * 1000 ) AS JPGold,AccountDate,Upid ,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal
 FROM  wagers_1.user_revenue_agent a  LEFT JOIN game.customer c ON c.Cid = a.HallId 
 WHERE a.AccountDate >= @start_time AND a.AccountDate < @end_time
 GROUP BY a.HallId,a.AccountDate,a.Currency,a.ExCurrency,a.CryDef,a.IsDemo ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold, RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;

-- game 

INSERT INTO wagers_1.game_revenue_player( Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,UserName,UpId,HallId,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal ) 
SELECT Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,UserName,UpId,HallId,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal 
FROM ( SELECT MD5(CONCAT_WS('-', GameId, Cid, LEFT(AddDate,15),Currency,ExCurrency,CryDef)) AS Id,count(Cid) AS Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,UserName,UpId,HallId,IsDemo,SUM( TRUNCATE( BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE( BetPoint / 1000,2) * 1000 ) AS BetPoint,SUM( TRUNCATE( WinGold / 1000,2) * 1000 ) AS WinGold,SUM( TRUNCATE( JPPoint / 1000,2) * 1000 ) AS JPPoint,SUM( TRUNCATE( JPGold / 1000,2) * 1000 ) AS JPGold,CONCAT(LEFT(AddDate,15),"0:00") AS AccountDate,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal
FROM  wagers_1.wagers_bet  
WHERE AddDate >= @start_time AND AddDate < @end_time 
AND IsValid = 1 
AND IsDemo = 0 AND GGId IN(1,2,3,4,5,6) GROUP BY GameId,Cid,LEFT(AddDate,15),Currency,ExCurrency,CryDef ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold , GameId=tempTable.GameId , RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;
 
 INSERT INTO wagers_1.game_revenue_agent( Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,HallId,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,RealBetGold,JPConGoldOriginal) 
 SELECT Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,HallId,IsDemo,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate ,RealBetGold,JPConGoldOriginal
 FROM ( SELECT MD5(CONCAT_WS('-', p.GameId, p.Cid, p.UpId,p.AccountDate,p.Currency,p.ExCurrency,p.CryDef)) AS Id,SUM(p.Rounds) AS Rounds,p.GGId,p.GameId,p.CryDef,p.Currency,p.ExCurrency,p.UpId AS Cid,p.HallId,p.IsDemo,SUM( TRUNCATE(p.BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE(p.BetPoint / 1000,2) * 1000 ) AS BetPoint,SUM( TRUNCATE(p.WinGold / 1000,2) * 1000 ) AS WinGold,SUM( TRUNCATE(p.JPPoint / 1000,2) * 1000 ) AS JPPoint,SUM( TRUNCATE(p.JPGold / 1000,2) * 1000 ) AS JPGold,p.AccountDate ,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal 
 FROM  wagers_1.game_revenue_player p  LEFT JOIN game.customer c ON c.Cid = p.UpId 
 WHERE p.AccountDate >= @start_time AND p.AccountDate < @end_time AND c.IsDemo = 0 AND GGId IN(1,2,3,4,5,6) GROUP BY p.GameId,p.UpId,p.AccountDate,p.Currency,p.ExCurrency,p.CryDef,c.IsDemo ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold , GameId=tempTable.GameId, RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;
 
 INSERT INTO wagers_1.game_revenue_hall( Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,Upid ,RealBetGold,JPConGoldOriginal)
 SELECT Id,Rounds,GGId,GameId,CryDef,Currency,ExCurrency,Cid,BetGold,BetPoint,WinGold,JPPoint,JPGold,AccountDate,Upid ,RealBetGold,JPConGoldOriginal
 FROM ( SELECT MD5(CONCAT_WS('-', a.GameId, a.Cid, a.HallId,a.AccountDate,a.Currency,a.ExCurrency,a.CryDef)) AS Id,SUM(a.Rounds) AS Rounds,a.GGId,a.GameId,a.CryDef,a.Currency,a.ExCurrency,a.HallId AS Cid,SUM( TRUNCATE(a.BetGold / 1000,2) * 1000 ) AS BetGold,SUM( TRUNCATE(a.BetPoint / 1000,2) * 1000) AS BetPoint,SUM( TRUNCATE(a.WinGold / 1000,2) * 1000) AS WinGold,SUM( TRUNCATE(a.JPPoint / 1000,2) * 1000) AS JPPoint,SUM( TRUNCATE(a.JPGold / 1000,2) * 1000) AS JPGold,AccountDate,Upid,SUM( TRUNCATE(RealBetGold / 1000,2) *1000 ) AS RealBetGold,SUM( TRUNCATE(JPConGoldOriginal /1000,4) * 1000 ) AS JPConGoldOriginal 
 FROM  wagers_1.game_revenue_agent a  LEFT JOIN game.customer c ON c.Cid = a.HallId  
 WHERE a.AccountDate >= @start_time AND a.AccountDate < @end_time GROUP BY a.GameId,a.HallId,a.AccountDate,a.Currency,a.ExCurrency,a.CryDef ) tempTable  ON DUPLICATE KEY UPDATE Rounds=tempTable.Rounds , BetGold=tempTable.BetGold , BetPoint=tempTable.BetPoint , WinGold=tempTable.WinGold , JPPoint=tempTable.JPPoint , JPGold=tempTable.JPGold , GameId=tempTable.GameId, RealBetGold=tempTable.RealBetGold, JPConGoldOriginal=tempTable.JPConGoldOriginal;
 
 
 call wagers_1.sp_checkoutGameRevenueHall(@start_time,@end_time);
 call wagers_1.sp_checkoutUserRevenueHall(@start_time,@end_time);
