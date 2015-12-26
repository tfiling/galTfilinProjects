package tokenizer;

public interface Message<T> {
	
	/**
	 * 
	 * @return message body
	 */
	public T getMessageBody();
	
	/**
	 * get the user's cookie if mentioned in a message header  
	 * @return user's cookie located in the cookie header
	 */
	public T getCookie();

	
	/**
	 * returns a string representation of the message
	 *
	 * @return the string
	 */
	@Override
	public String toString();
}
