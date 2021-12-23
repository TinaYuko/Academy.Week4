/*
 Creare un nuovo database Ticketing con una sola tabella Tickets. Le colonne sono:
ID (int, PK, auto-incrementale)
Descrizione (varchar(500))
Data (datetime)
Utente (varchar(100))
Stato (varchar(10)) – New, OnGoing, Resolved



Realizzare una Console app che acceda al database Ticketing utilizzando il Connected Mode di ADO.NET e che:
Stampi la lista dei Ticket in ordine cronologico (dal più recente al più vecchio)
Permetta l'inserimento di nuovi Ticket (i dati devono essere inseriti dall'utente)
Permetta la cancellazione di un Ticket (utilizzare l'ID univoco per identificarlo)
*/

using Esercitazione_1;

Menu.Start();


