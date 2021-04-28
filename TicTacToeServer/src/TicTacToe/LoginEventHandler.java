package TicTacToe;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class LoginEventHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {
		// TODO Auto-generated method stub
		String username = (String) arg0.getParameter(SFSEventParam.LOGIN_NAME);
		String password = (String) arg0.getParameter(SFSEventParam.LOGIN_PASSWORD);

		ISession session = (ISession) arg0.getParameter(SFSEventParam.SESSION);

		// Build a prepared statement
		try {

			// get a connection to the database
			Connection conn = getParentExtension().getParentZone().getDBManager().getConnection();

			// This will strip potential SQL injections
//			PreparedStatement sql = conn.prepareStatement("SELECT * FROM UserInfo WHERE USERNAME = ?");
			PreparedStatement sql = conn.prepareStatement("exec CheckAcc ?");
			sql.setString(1, username);
//			sql.setString(2, password);

			// Obtain ResultSet
			ResultSet result = sql.executeQuery();

			ISFSArray row = SFSArray.newFromResultSet(result);

			// make sure there is a password before you try to use the checkSecurePassword
			// function
			if (password.equals("")) {
				SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
				data.addParameter(username);
				throw new SFSLoginException("You must enter a password.", data);
			}

			// SFS always encrypts passwords before sending them so you need to decrypt the
			// password
			// received from the database and compare that to what they entered in flash
			if (row.size() == 0
					|| !getApi().checkSecurePassword(session, row.getSFSObject(0).getUtfString("PassAcc"), password)) {
				SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);

				data.addParameter(username);

				throw new SFSLoginException("Login failed for user: " + username, data);
			}

			// this was in one of the SFS examples so I left it in there for testing
			// purposes
			if (username.equals("Gonzo") || username.equals("Kermit")) {

				// Create the error code to send to the client
				SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
				errData.addParameter(username);

				// Fire a Login exception
				throw new SFSLoginException("Gonzo and Kermit are not allowed in this Zone!", errData);
			}

			// make sure you close the database connection when you're done with it,
			// especially if you've
			// set a low number of maximum connections
			conn.close();

			// at this point you could trigger an joinRoom request if you wanted to,
			// otherwise
			// this will return success to your LOGIN event listener
			trace("Login successful, joining room!");
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
