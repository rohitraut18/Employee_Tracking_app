package com.be.employeetrackingapp.employee;

import androidx.appcompat.app.AppCompatActivity;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.be.employeetrackingapp.App;
import com.be.employeetrackingapp.R;
import com.be.employeetrackingapp.account.LoginActivity;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationCallback;
import com.google.android.gms.location.LocationRequest;
import com.nabinbhandari.android.permissions.PermissionHandler;
import com.nabinbhandari.android.permissions.Permissions;

import java.util.ArrayList;

public class EMainActivity extends AppCompatActivity {

    FusedLocationProviderClient fusedLocationClient;
    LocationRequest locationRequest;
    LocationCallback locationCallback;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_e_main);

        RequestLocationPermissions();
    }

    void RequestLocationPermissions(){
        String[] permissions = {Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION};
        String rationale = "Please provide location permission.";
        Permissions.Options options = new Permissions.Options()
                .setRationaleDialogTitle("Info")
                .setSettingsDialogTitle("Warning");

        Permissions.check(this, permissions, rationale, options, new PermissionHandler() {
            @Override
            public void onGranted() {
                Intent intent = new Intent(EMainActivity.this, UpdateLocationService.class);
                startService(intent);
            }

            @Override
            public void onDenied(Context context, ArrayList<String> deniedPermissions) {
                RequestLocationPermissions();
            }
        });
    }

    public void btnMyAccount_Click(View view) {
    }

    public void btnLogout_OnClick(View view) {
        App.ClearLoggedUser(getApplicationContext());
        finish();

        Intent service = new Intent(getApplicationContext(), UpdateLocationService.class);
        stopService(service);

        Intent intent = new Intent(getApplicationContext(), LoginActivity.class);
        startActivity(intent);
    }


}
