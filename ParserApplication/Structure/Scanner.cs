using ParserApplication.TokenConstruction;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

public class Scanner {
    //El scanner lee token por token
    //Llamadas consecutivas a getToken por eso se realiza global 
    private string _regexp = "";
    private int _index = 0;
    private int _state = 0;
    private const char EOF = (char)0;

    /*Se le agrega un caracter al final para saber que se finalizo, es el delimitador
    para no leer toda la cadena para encontrar el final
         */

    public Scanner ( string regexp ) {
        _regexp = regexp + (char)TokenType.EOF;
        _index = 0;
        _state = 0;
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
                        peek = _regexp [_index];
                    }

                    switch (peek) {
                        case '\\':
                            _state = 1;
                            break;
                        case (char)TokenType.apostrofe:
                            result.Value += peek.ToString();
                            _index++;
                            peek = _regexp [_index];
                            while (char.IsLetterOrDigit(peek)) {
                                tokenFound = true;
                                result.Tag = TokenType.term;
                                result.Value += peek.ToString();
                                _index++;
                                peek = _regexp [_index];
                            }
                            if ((char)TokenType.apostrofe == peek) {
                                result.Value += peek.ToString();
                            } else { MessageBox.Show("Error de lexema"); }
                            break;
                        case (char)TokenType.puntoycoma:
                        case (char)TokenType.igual:
                        case (char)TokenType.or:
                        case (char)TokenType.EOF:
                            tokenFound = true;
                            result.Tag = (TokenType)peek;
                            break;
                        default:
                            if (char.IsLetterOrDigit(peek)) {
                                while (char.IsLetterOrDigit(peek)) {
                                    tokenFound = true;
                                    result.Tag = TokenType.id;
                                    result.Value += peek.ToString();
                                    _index++;
                                    peek = _regexp [_index];
                                }
                                _index--;
                            } else { MessageBox.Show("Error de lexema"); }

                            break;
                    } //Switch peek

                    break; // case state0
                case 1:
                    switch (peek) {
                        case (char)TokenType.apostrofe:
                            result.Value += peek.ToString();
                            _index++;
                            peek = _regexp [_index];
                            while (char.IsLetterOrDigit(peek)) {
                                tokenFound = true;
                                result.Tag = TokenType.term;
                                result.Value += peek.ToString();
                                _index++;
                                peek = _regexp [_index];
                            }
                            if ((char)TokenType.apostrofe == peek) {
                                result.Value += peek.ToString();
                            } else { MessageBox.Show("Error de lexema, se esperaba una apostrofe"); }
                            break;
                        case (char)TokenType.puntoycoma:
                        case (char)TokenType.igual:
                        case (char)TokenType.or:
                        case '\\':
                        case ' ':
                            if (char.IsLetterOrDigit(peek)) {
                                while (char.IsLetterOrDigit(peek)) {
                                    tokenFound = true;
                                    result.Tag = TokenType.id;
                                    result.Value += peek.ToString();
                                    _index++;
                                    peek = _regexp [_index];
                                }
                                _index--;
                            } else { MessageBox.Show("Error de lexema"); }
                            break;
                        default:
                            MessageBox.Show("Error de lexema");
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
