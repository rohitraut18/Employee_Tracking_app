package com.be.employeetrackingapp;

import android.content.Context;
import android.content.SharedPreferences;

import com.be.employeetrackingapp.models.AppUser;
import com.be.employeetrackingapp.models.UserCredentials;

import java.util.HashMap;

public class App {
    public static String WebAppUrl=  "http://192.168.0.104:8081/EmployeeTracking/";
    public static String GoogleAPIKey=  "AIzaSyAnn3YDpblUWVPQGe2n7zIBVUdsfmplOtU";

    public static AppUser LoggedUser;

    public static HashMap<String,String> getCookies(String CookieData){
        HashMap<String,String> cookies = new HashMap<String, String>();

        String[] cks = CookieData.split(";");
        for (int i=0; i<cks.length; i++){
            String[] kv = cks[i].split("=");

            String key = kv[0].trim();
            String value = "";

            if (kv.length > 1)
                value = kv[1];

            cookies.put(key,value);
        }

        return cookies;
    }

    public static void SetLoggedUser(Context context, String Username, String Password){
        SharedPreferences.Editor editor = context.getSharedPreferences("LoggedUser", Context.MODE_PRIVATE).edit();
        editor.putString("Username", Username);
        editor.putString("Password", Password);
        editor.apply();
    }

    public static UserCredentials GetLoggedUser(Context context){
        SharedPreferences prefs  = context.getSharedPreferences("LoggedUser", Context.MODE_PRIVATE);
        String Username = prefs.getString("Username", null);
        String Password = prefs.getString("Password", null);

        if (Username == null)
            return null;

        UserCredentials credentials = new UserCredentials();
        credentials.Username = Username;
        credentials.Password = Password;
        return credentials;
    }

    public static void ClearLoggedUser(Context context){
        SharedPreferences.Editor editor = context.getSharedPreferences("LoggedUser", Context.MODE_PRIVATE).edit();
        editor.clear();
        editor.apply();
    }
}
