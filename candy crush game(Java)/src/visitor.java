
public interface visitor {

	public void visit(RegularCandy other);
	
	public void visit(HorizontalStripedCandy horizontalStripedCandy);

	public void visit(VerticalStripedCandy verticalStripedCandy);

	public void visit(BagCandy bagCandy);

	public void visit(BombCandy bombCandy);

}
