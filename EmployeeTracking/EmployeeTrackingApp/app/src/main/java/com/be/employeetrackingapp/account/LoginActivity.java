package com.be.employeetrackingapp.account;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.Toast;

import com.be.employeetrackingapp.App;
import com.be.employeetrackingapp.R;
import com.be.employeetrackingapp.employee.EMainActivity;
import com.be.employeetrackingapp.models.AppUser;
import com.google.gson.JsonObject;
import com.koushikdutta.async.future.FutureCallback;
import com.koushikdutta.async.http.Headers;
import com.koushikdutta.ion.HeadersResponse;
import com.koushikdutta.ion.Ion;
import com.koushikdutta.ion.Response;

import java.util.HashMap;

public class LoginActivity extends AppCompatActivity {

    EditText txtEmailId;
    EditText txtPassword;
    Button btnLogin;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        txtEmailId = findViewById(R.id.txtEmailId);
        txtPassword = findViewById(R.id.txtPassword);
        btnLogin = findViewById(R.id.btnLogin);
    }

    public void btnLogin_OnClick(View view) {
        btnLogin.setText("Please wait");
        String url = App.WebAppUrl + "Account/Login";

        //get string in another string
        String username =  txtEmailId.getText().toString();
        String password =  txtPassword.getText().toString();

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
                            btnLogin.setText("Login");
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

                        //Save  login credentials to SharedPreferences
                        App.SetLoggedUser(getApplicationContext(), username, password);


                        Intent intent = new Intent(getApplicationContext(), EMainActivity.class);
                        startActivity(intent);

                        finish();
                        Toast.makeText(getApplicationContext(), "Welcome, " + u.FullName, Toast.LENGTH_LONG).show();

                    }
                });
    }
}
