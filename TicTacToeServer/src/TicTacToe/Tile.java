package TicTacToe;

public enum Tile {
	Tic(1),
	Tac(2),
	Empty(0);
	
	private final int _value;

	Tile(final int newValue) {
		_value = newValue;
    }

    public int getValue() { return _value; }
}
