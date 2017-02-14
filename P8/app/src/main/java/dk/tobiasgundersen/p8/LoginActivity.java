package dk.tobiasgundersen.p8;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class LoginActivity extends AppCompatActivity {
    interface Test {
        int run(int x);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        Test test = (x -> 2 * x);
        Test test3 = (x -> 3 * x);
    }
}
