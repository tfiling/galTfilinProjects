package tokenizer_http;

import java.util.Map;

public class GetRequestMessage extends RequestMessage {

	public GetRequestMessage(String uri, String version, Map<String, String> headers, String body) {
		super(uri, version, headers, body);
	}


	@Override
	public boolean checkLegalMessage() {
		if (this.uri.length() == 0 || this.version.length() == 0 || this.body.length() != 0)
		{
			return false;
		}
		else return true;
	}

}
