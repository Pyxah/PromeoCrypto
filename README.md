# PromeoCrypto

<b>1. Présentation du projet</b>

Ce projet est constitué d'un serveur et d'un client qui communiquent via un réseau TCP/IP. L'objectif principal de ce projet est de permettre au client de chiffrer du texte en utilisant l'une des trois méthodes de chiffrement : César, Playfair et Substitution.

Le texte à chiffrer est envoyé au serveur, qui effectue le chiffrement et renvoie le texte chiffré au client.

Voici le flux de travail général :

- L'utilisateur saisit du texte dans l'interface utilisateur du client.
- L'utilisateur sélectionne une méthode de chiffrement.
- L'utilisateur envoie une demande au serveur pour chiffrer le texte.
- Le serveur chiffre le texte en utilisant la méthode de chiffrement spécifiée.
- Le serveur renvoie le texte chiffré au client.
- Le client affiche le texte chiffré.

<b>2. Serveur</b>

Le serveur est une application console C# qui écoute les connexions entrantes sur le port 6666. Lorsqu'une connexion est établie, le serveur crée un nouveau thread pour gérer la communication avec le client. Cela permet au serveur de gérer plusieurs connexions simultanément.

Le serveur utilise trois méthodes de chiffrement différentes pour chiffrer le texte :
- Chiffrement César : Cette méthode de chiffrement par substitution décale chaque lettre du texte en clair d'un certain nombre de places dans l'alphabet. Par défaut, le décalage est de 3 positions.
- Chiffrement Playfair : Cette méthode de chiffrement utilise une matrice 5x5 de lettres, connue sous le nom de clé de chiffrement, pour chiffrer le texte en clair en paires de lettres.
- Chiffrement par substitution : Cette méthode utilise un tableau de substitution pour transformer chaque lettre du texte en clair.

Le serveur chiffre le texte en fonction du type de chiffrement spécifié par le client. Si le client envoie un type de chiffrement non reconnu, le serveur renvoie une erreur.

Enfin, le serveur envoie le texte chiffré au client et ferme la connexion.


<b>3. Client</b>

Le client est une application WPF (Windows Presentation Foundation) C# qui permet à l'utilisateur de saisir du texte à chiffrer et de choisir une méthode de chiffrement.

L'utilisateur peut choisir l'une des trois méthodes de chiffrement : César, Playfair et Substitution. Une fois le texte et la méthode de chiffrement sélectionnés, l'utilisateur peut envoyer une demande de chiffrement au serveur en appuyant sur la touche "Entrer".

Le client envoie la méthode de chiffrement et le texte à chiffrer au serveur via une connexion TCP/IP. Le serveur chiffre le texte et renvoie le texte chiffré, que le client affiche dans l'interface utilisateur.


<b>4. Comment utiliser le projet</b>

  1- Lancez le serveur en lançant le projet serveur dans un IDE C# comme Visual Studio.

  2- Lancez le client en lançant le projet client dans un IDE C#.

  3- Dans l'interface utilisateur du client, saisissez le texte que vous souhaitez chiffrer dans la zone de texte "Phrase à chiffrer". Assurez-vous que le texte ne contient que des lettres alphabétiques en majuscules et des espaces.

  4- Sélectionnez la méthode de chiffrement souhaitée en cochant l'une des options : Code César, Playfair ou Substitution.

  5- Appuyez sur la touche Entrée de votre clavier pour envoyer la demande de chiffrement au serveur.

  6- Attendez la réponse du serveur. Une fois le chiffrement effectué, le texte chiffré sera affiché dans la zone de texte "Phrase chiffrée" de l'interface utilisateur du client.

  7- Répétez les étapes 3 à 6 pour chiffrer d'autres textes avec différentes méthodes de chiffrement.

  8- Lorsque vous avez terminé, fermez l'application client en appuyant sur le bouton “Quitter”.


<b>5. Gestion des erreurs</b>

Le projet client et serveur intègre une gestion des erreurs pour traiter les problèmes qui pourraient survenir lors de la communication ou du chiffrement.

Si une erreur réseau se produit lors de l'envoi ou de la réception des données entre le client et le serveur, une boîte de message d'erreur s'affiche pour informer l'utilisateur de l'erreur.

De plus, si le client ferme la connexion de manière inattendue, le serveur affiche le message "Connexion fermée par le client" pour indiquer que la connexion a été fermée.

Enfin, le projet gère également les erreurs de caractères.


<b>6. Conclusion</b>

Le projet CryptoServer est une application client-serveur qui permet de chiffrer du texte en utilisant différentes méthodes de chiffrement. Le serveur gère les demandes de chiffrement, effectue le chiffrement et renvoie le texte chiffré au client. Le client offre une interface conviviale pour saisir le texte et choisir la méthode de chiffrement.
