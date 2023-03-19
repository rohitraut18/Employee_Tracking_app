package com.be.employeetrackingapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.be.employeetrackingapp.account.LoginActivity;
import com.be.employeetrackingapp.employee.EMainActivity;
import com.be.employeetrackingapp.models.AppUser;
import com.be.employeetrackingapp.models.UserCredentials;
import com.google.gson.JsonObject;
import com.koushikdutta.async.future.FutureCallback;
import com.koushikdutta.async.http.Headers;
import com.koushikdutta.ion.HeadersResponse;
import com.koushikdutta.ion.Ion;
import com.koushikdutta.ion.Response;

import java.util.HashMap;

public class MainActivity extends AppCompatActivity {

    TextView lblMsg;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        lblMsg = findViewById(R.id.lblMsg);
        lblMsg.setText("");

        //Check for user credentials in getSharedPreferences
        UserCredentials credentials = App.GetLoggedUser(getApplicationContext());
        if (credentials == null)
        {
            finish();
            Intent intent = new Intent(getApplication(), LoginActivity.class);
            startActivity(intent);
            return;
        }

        Login(credentials);
    }

    public void Login(UserCredentials credentials) {
        lblMsg.setText("Please wait");
        String url = App.WebAppUrl + "Account/Login";

        //get string in another string
        String username =  credentials.Username;
        String password =  credentials.Password;

        Ion.with(getApplicationContext())
                .load(url)
                .setBodyParameter("Username", username)
                .setBodyParameter("Password", password)
                .asJsonObject()
                .withResponse()
                .setCallback(new FutureCallback<Response<JsonObject>>() {
                    @Override
                    public void onCompleted(Exception e, Response<JsonObject> result) {

                        if(e != null)
                        {
                            lblMsg.setText("");
                            Toast.makeText(getApplicationContext(), "Error : " + e.getMessage(), Toast.LENGTH_LONG).show();
                            return;
                        }

                        HeadersResponse headersResponse = result.getHeaders();
                        JsonObject data = result.getResult();

                        String status = data.get("status").getAsString();

                        if (status.equals("error")){
                            String msg = data.get("message").getAsString();
                            Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_LONG).show();
                            return;
                        }

                        //Get Authentication Cookie
                        Headers headers = headersResponse.getHeaders();
                        String CookieData = headers.get("Set-Cookie");
                        HashMap<String,String> Cookies = App.getCookies(CookieData);
                        String AuthCookie = Cookies.get(".ASPXAUTH");

                        JsonObject jsu = data.getAsJsonObject("data");
                        int AppUserId = jsu.get("AppUserId").getAsInt();
                        String FullName = jsu.get("FullName").getAsString();
                        String MobileNo = jsu.get("MobileNo").getAsString();
                        String EmailId = jsu.get("EmailId").getAsString();

                        AppUser u = new AppUser();
                        u.AuthCookie = AuthCookie;
                        u.AppUserId = AppUserId;
                        u.FullName = FullName;
                        u.MobileNo = MobileNo;
                        u.EmailId = EmailId;

                        App.LoggedUser = u;


                        Intent intent = new Intent(getApplication(), EMainActivity.class);
                        startActivity(intent);


                        finish();
                        Toast.makeText(getApplicationContext(), "Welcome, " + u.FullName, Toast.LENGTH_LONG).show();

                    }
                });
    }

    public void btnLoginOrChangeAccount_OnClick(View view) {
        finish();
        Intent intent = new Intent(getApplication(), LoginActivity.class);
        startActivity(intent);
    }
}
