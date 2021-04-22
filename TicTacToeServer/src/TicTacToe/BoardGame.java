package TicTacToe;

public final class BoardGame {

	private static final BoardGame _instance;

	// Static block initialization for exception handling
	static {
		try {
			_instance = new BoardGame();
		} catch (Exception e) {
			throw new RuntimeException("Exception occured in creating singleton instance");
		}
	}

	public static BoardGame Instance() {
		return _instance;
	}

	public static final int squareSize = 3;

	private final Tile[][] _boardData;

	private int _moveCount = 0;

	private BoardGame() {
		_boardData = new Tile[squareSize][squareSize];
		for (int i = 0; i < squareSize; i++) {
			for (int j = 0; j < squareSize; j++) {
				_boardData[i][j] = Tile.Empty;
			}
		}
	}

	private boolean IsEmpty(int x, int y) {
		return (_boardData[x][y] == Tile.Empty) ? true : false;
	}

	public Tile[][] getBoardData() {
		return _boardData;
	}

	public void setBoardData(int x, int y, Tile value) {
		if (x < 0 || x > 3 || y < 0 || y > 3) {
			System.out.println("Out range of matrix");
			return;
		}
		_boardData[x][y] = value;
	}

	public Tile getBoardData(int x, int y) {
		if (x < 0 || x > 3 || y < 0 || y > 3) {
			System.out.println("Out range of matrix");
		}
		return _boardData[x][y];
	}

	public int getMoveCount() {
		return _moveCount;
	}

	public void setMoveCount(int _moveCount) {
		this._moveCount = _moveCount;
	}

	public void increaseMoveCount() {
		this._moveCount++;
	}

	public Boolean[] getBoardDataEmpty() {
		Boolean[] ret = new Boolean[squareSize * squareSize];
		for (int i = 0; i < _boardData.length; i++) {
			for (int j = 0; j < _boardData[i].length; j++) {
				ret[i * squareSize + j] = IsEmpty(i, j);
			}
		}
		return ret;
	}
}
