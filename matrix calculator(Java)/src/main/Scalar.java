package main;

public interface Scalar {

	public Scalar add(Scalar s);
	
	public Scalar mul(Scalar s);
	
	public Scalar neg();
	
	public Scalar inv();
	
	public void setValue(Scalar s);
	
	public String toString();
	
	public String toStringValue();

	public boolean equalzero(Rational zero);
	
	public void minimize();
	
}
