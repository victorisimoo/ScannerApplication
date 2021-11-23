using ParserApplication.TokenConstruction;
using System.Windows.Forms;

public class Scanner {
    //El scanner lee token por token
    //Llamadas consecutivas a getToken por eso se realiza global 
    private string _regexp = "";
    private int _index = 0;
    private int _state = 0;
    private const char EOF = (char)0;
    string resultado;
    bool error = false;

    /*Se le agrega un caracter al final para saber que se finalizo, es el delimitador
    para no leer toda la cadena para encontrar el final
         */

    public Scanner ( string regexp ) {
        error = false;
        _regexp = regexp + (char)TokenType.EOF;
        _index = 0;
        _state = 0;
    }

    public string getResult () {
        return this.resultado;
    }

    public bool getErrorResult () {
        return this.error;
    }


    // a un token se le asocia un valor y un nombre, por lo tanto 2 campos que devuelve un tipo de dato
    public Token GetToken () {
        Token result = new Token() { Value = "" };//vacio inicia
        bool tokenFound = false;
        while (!tokenFound) {
            char peek = _regexp [_index];

            switch (_state) {

                case 0:

                    // Whitespace removal
                    while (char.IsWhiteSpace(peek)) {
                        _index++;
                        peek = _regexp[_index];
                    }

                    switch (peek) {
                        case '\\':
                            _state = 1;
                            break;
                        case (char)TokenType.apostrofe:
                            result.Value += peek.ToString();
                            _index++;
                            peek = _regexp[_index];
                            while (char.IsWhiteSpace(peek) || char.IsLetterOrDigit(peek) || peek >= 33 && peek <= 38 || peek >= 40 && peek <= 47 || peek >= 58 && peek <= 63 || peek >= 123 && peek <= 126 || peek >= 123 && peek <= 126 || peek >= 93 && peek <= 95 || peek == '[' || peek == 10 || peek == 9 || peek == 92)
                            {
                                tokenFound = true;
                                result.Tag = TokenType.term;
                                result.Value += peek.ToString();
                                _index++;
                                peek = _regexp [_index];
                            }
                            if ((char)TokenType.apostrofe == peek) {
                                result.Value += peek.ToString();
                                resultado = resultado + result.Value;
                            } 
                            else {
                                if (error != true) {
                                    error = true;
                                    MessageBox.Show("Error en el lexema encontrado.", "¡Error encontrado!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            break;
                        case (char)TokenType.puntoycoma:
                        case (char)TokenType.igual:
                        case (char)TokenType.or:
                        case (char)TokenType.EOF:
                            tokenFound = true;
                            result.Tag = (TokenType)peek;
                            break;
                        default:
                            if (char.IsLetter(peek) || peek == '_') {
                                while (char.IsLetterOrDigit(peek) || peek == '_') {
                                    tokenFound = true;
                                    result.Tag = TokenType.id;
                                    result.Value += peek.ToString();
                                    _index++;
                                    peek = _regexp [_index];
                                }
                                _index--;
                            } else {
                                if (error != true) {
                                    error = true;
                                    MessageBox.Show("Error en el lexema encontrado.", "¡Error encontrado!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                               
                            }
                            break;
                    } //Switch peek

                    break; // case state0
                case 1:
                    switch (peek) {
                        case (char)TokenType.apostrofe:
                            result.Value += peek.ToString();
                            _index++;
                            peek = _regexp [_index];
                            while (char.IsWhiteSpace(peek) || char.IsLetterOrDigit(peek) || peek >= 33 && peek <= 38 || peek >= 40 && peek <= 47 || peek >= 58 && peek <= 63 || peek >= 123 && peek <= 126 || peek >= 123 && peek <= 126 || peek >= 93 && peek <= 95 || peek == '[' || peek == 10 || peek == 9 || peek == 92) {
                                tokenFound = true;
                                result.Tag = TokenType.term;
                                result.Value += peek.ToString();
                                _index++;
                                peek = _regexp [_index];
                            }
                            if ((char)TokenType.apostrofe == peek) {
                                result.Value += peek.ToString();
                            } else {
                                if (error != true) {
                                    error = true;
                                    MessageBox.Show("Error en el lexema encontrado.", "¡Error encontrado!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            break;
                        case (char)TokenType.puntoycoma:
                        case (char)TokenType.igual:
                        case (char)TokenType.or:
                        case '\\':
                        case ' ':
                            if (char.IsLetter(peek) || peek == '_') {
                                while (char.IsLetterOrDigit(peek) || peek == '_') {
                                    tokenFound = true;
                                    result.Tag = TokenType.id;
                                    result.Value += peek.ToString();
                                    _index++;
                                    peek = _regexp [_index];
                                }
                                _index--;
                            } else {
                                if (error != true) {
                                    error = true;
                                    MessageBox.Show("Error en el lexema encontrado.", "¡Error encontrado!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            break;
                        default:
                            if (error != true) {
                                error = true;
                                MessageBox.Show("Error en el lexema encontrado.", "¡Error encontrado!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                    }
                    break; //case state1

                default:
                    break;
            } //Switch state

            _index++;

        } //While -TokenFound
        _state = 0;

        return result;

    }//GET Token
}
