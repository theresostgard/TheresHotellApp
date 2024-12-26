use TheresHotell

--Visa alla bokningar inklusive bokningsnr, ankomst- och avresedag, antal gäster, rumsnr,
-- förnamn och efternamn på den bokande gästen, gästens gästId och antal bokade rum
SELECT 
    b.BookingId AS 'BokningsNr', 
    FORMAT(b.ArrivalDate, 'yyyy-MM-dd') AS 'Ankomstdag', 
    FORMAT(b.DepartureDate, 'yyyy-MM-dd') AS 'Avresedag', 
    b.AmountOfGuests AS 'Antal Gäster', 
    r.RoomId AS 'RumsNr',
    g.FirstName AS 'Förnamn',
    g.LastName AS 'Efternamn',
    g.GuestId AS 'GästNr',
    (SELECT COUNT(*) 
     FROM BookingRoom br 
     WHERE br.BookingId = b.BookingId) AS 'Antal Bokade Rum'
FROM 
    Booking b
JOIN 
    Guest g ON g.GuestId = b.GuestId
JOIN 
    BookingRoom br ON b.BookingId = br.BookingId
JOIN 
    Room r ON br.RoomId = r.RoomId
ORDER BY 
    ArrivalDate;

--Visa antal bokningar per gäst
USE TheresHotell
SELECT 
    g.FirstName AS 'Förnamn', 
    g.LastName AS 'Efternamn',
    (SELECT COUNT(*) 
     FROM Booking b 
     WHERE b.GuestId = g.GuestId) AS 'Antal bokningar'
FROM 
    Guest g;

	--Visa antal bokningar per gäst inkl GästId och i fallande ordning.
USE TheresHotell
SELECT 
    g.GuestId AS 'GästNr',
    g.FirstName AS 'Förnamn',
    g.LastName AS 'Efternamn',
    COUNT(b.BookingId) AS 'Antal Bokningar'
FROM 
    Guest g
JOIN 
    Booking b ON g.GuestId = b.GuestId
GROUP BY 
    g.GuestId, g.FirstName, g.LastName
ORDER BY 
    'Antal Bokningar' DESC;

--Visa antal bokningar i genomsnitt
USE TheresHotell
SELECT 
	FORMAT(b.ArrivalDate, 'yyyy-MM') AS 'Månad',
	AVG(b.AmountOfGuests) AS 'Antal bokningar i genomsnitt'
FROM 
    Booking b
GROUP BY 
    FORMAT(b.ArrivalDate, 'yyyy-MM')
ORDER BY 
    'Månad';

--Visa antal rum grupperade i storleksordning i stigande ordning där rumstypen är dubbelrum och rummet är markerat som aktivt. 
USE TheresHotell
SELECT 
    r.RoomSize AS 'Rumstorlek', 
    COUNT(*) AS 'Antal Rum'
FROM 
    Room r
WHERE 
    r.RoomType = 'Double' 
    AND r.Status = 'Active'
GROUP BY 
    r.RoomSize;