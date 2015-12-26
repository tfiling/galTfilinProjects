package tokenizer_whatsaap;

import tokenizer.Tokenizer;
import tokenizer.TokenizerFactory;
import tokenizer_http.HttpTokenizer;

public class WhatsAppTokenizerFactory implements TokenizerFactory {

	@Override
	public Tokenizer create() {
		return new HttpTokenizer(); 
	}

}
