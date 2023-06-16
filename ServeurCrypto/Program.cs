using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    private Socket listener;
    private const int port = 6666;

    /*
     *  type de chiffrement par substitution dans lequel chaque lettre dans le texte en clair est 'décalée' un certain nombre 
     *  de places plus loin dans l'alphabet. Dans ce cas, le décalage est de 3 positions. 
     */
    private string CaesarCipher(string text, int shift = 3)
    {
        // Convertir la chaîne en majuscules
        text = text.ToUpper();

        char[] buffer = text.ToCharArray();

        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];

            // Si c'est une lettre
            if (Char.IsLetter(letter))
            {
                // décaler les lettres 3 places plus loin
                letter = (char)(letter + shift);

                // boucler à 'A' si on a dépassé 'Z'
                if (letter > 'Z')
                {
                    letter = (char)(letter - 26);
                }
                // boucler à 'Z' si on est avant 'A'
                else if (letter < 'A')
                {
                    letter = (char)(letter + 26);
                }

                buffer[i] = letter;
            }
        }
        
        Console.WriteLine($"Chiffrement de César effectué sur le texte : {text}");
        return new string(buffer);
    }

    /*
     * Cette fonction convertit d'abord le texte en majuscules et ajoute un 'X' à la fin si nécessaire pour obtenir une longueur 
     * de texte paire. Ensuite, elle utilise une boucle for pour parcourir chaque paire de lettres du texte. Pour chaque paire, 
     * elle trouve leurs positions dans la clé, puis ajoute les lettres chiffrées correspondantes à la chaîne de sortie.
     */
    private string PlayfairCipher(string text)
    {
        // Convertir le texte en majuscules
        text = text.ToUpper();

        // Si la longueur du texte est impaire, ajoutez un X à la fin
        if (text.Length % 2 != 0)
        {
            text += "X";
        }

        // Définir la clé de chiffrement
        char[,] keySquare = {
        {'B', 'Y', 'D', 'G', 'Z'},
        {'J', 'S', 'F', 'U', 'P'},
        {'L', 'A', 'R', 'K', 'X'},
        {'C', 'O', 'I', 'V', 'E'},
        {'Q', 'N', 'M', 'H', 'T'}
    };

        // Initialiser la chaîne de sortie
        string cipherText = "";

        // Chiffrer le texte par paires de lettres
        for (int i = 0; i < text.Length; i += 2)
        {
            char char1 = text[i] == 'W' ? 'X' : text[i]; // Si le caractère est W, le remplacer par X
            char char2 = text[i + 1] == 'W' ? 'X' : text[i + 1]; // Si le caractère est W, le remplacer par X

            int row1 = 0, col1 = 0, row2 = 0, col2 = 0;

            // Trouver les positions des caractères dans la clé
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (keySquare[row, col] == char1)
                    {
                        row1 = row;
                        col1 = col;
                    }
                    else if (keySquare[row, col] == char2)
                    {
                        row2 = row;
                        col2 = col;
                    }
                }
            }

            // Si les lettres sont sur la même rangée, prenez les lettres à droite
            if (row1 == row2)
            {
                cipherText += keySquare[row1, (col1 + 1) % 5];
                cipherText += keySquare[row2, (col2 + 1) % 5];
            }
            // Si les lettres sont dans la même colonne, prenez les lettres en dessous
            else if (col1 == col2)
            {
                cipherText += keySquare[(row1 + 1) % 5, col1];
                cipherText += keySquare[(row2 + 1) % 5, col2];
            }
            // Sinon, prenez les lettres aux coins opposés du rectangle
            else
            {
                cipherText += keySquare[row1, col2];
                cipherText += keySquare[row2, col1];
            }
        }
        Console.WriteLine($"Chiffrement de Playfair effectué sur le texte : {text}");
        return cipherText;
    }


    /*
     * Cette fonction utilise un Dictionary<char, char> pour stocker le tableau de substitution. 
     * Il transforme chaque caractère de la chaîne en entrée en utilisant le dictionnaire et construit la chaîne de sortie. 
     * Les caractères non alphabétiques sont conservés tels quels.
     */
    private string SubstitutionCipher(string text)
    {
        // Convertir le texte en majuscules
        text = text.ToUpper();

        // Initialiser le tableau de substitution
        Dictionary<char, char> substitutionTable = new Dictionary<char, char>()
    {
        {'A', 'H'}, {'B', 'I'}, {'C', 'J'}, {'D', 'K'}, {'E', 'L'}, {'F', 'M'}, {'G', 'N'}, {'H', 'V'}, {'I', 'W'},
        {'J', 'X'}, {'K', 'Y'}, {'L', 'Z'}, {'M', 'B'}, {'N', 'C'}, {'O', 'A'}, {'P', 'D'}, {'Q', 'E'}, {'R', 'F'},
        {'S', 'G'}, {'T', 'O'}, {'U', 'P'}, {'V', 'Q'}, {'W', 'R'}, {'X', 'S'}, {'Y', 'T'}, {'Z', 'U'}
    };

        // Initialiser la chaîne de sortie
        string cipherText = "";

        // Chiffrer le texte
        foreach (char c in text)
        {
            if (Char.IsLetter(c))
            {
                cipherText += substitutionTable[c];
            }
            else
            {
                cipherText += c;
            }
        }

        Console.WriteLine($"Chiffrement par substitution effectué sur le texte : {text}");
        return cipherText;
    }


    public void Start()
    {
        // Création du socket d'écoute
        listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, port));
        listener.Listen(5);

        while (true)
        {
            Console.WriteLine("Attente client...");
            Socket clientSocket = listener.Accept();
            Thread thread = new Thread(new ParameterizedThreadStart(HandleClient));
            thread.Start(clientSocket);
        }
    }

    private void HandleClient(object obj)
    {
        Socket clientSocket = obj as Socket;

        // Obtention des informations sur le client connecté
        IPEndPoint clientEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;

        if (clientEndPoint != null)
        {
            // Affichage des informations de connexion
            Console.WriteLine($"{DateTime.Now} - {clientEndPoint.Address}:{clientEndPoint.Port} - Attente de la demande de chiffrement");

            byte[] buffer = new byte[1024];
            int size = clientSocket.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, size);

            // Extraction du type de chiffrement et du texte à chiffrer
            string cipherType = message.Substring(0, 1);
            string clearText = message.Substring(2);

            // Chiffrement du texte en fonction du type de chiffrement
            string cipherText;
            switch (cipherType)
            {
                case "C":
                    cipherText = CaesarCipher(clearText);
                    break;
                case "P":
                    cipherText = PlayfairCipher(clearText);
                    break;
                case "S":
                    cipherText = SubstitutionCipher(clearText);
                    break;
                default:
                    cipherText = "Type de chiffrement non reconnu";
                    break;
            }

            // Envoi du texte chiffré au client
            clientSocket.Send(Encoding.ASCII.GetBytes(cipherText));
        }

        clientSocket.Close();
    }
}