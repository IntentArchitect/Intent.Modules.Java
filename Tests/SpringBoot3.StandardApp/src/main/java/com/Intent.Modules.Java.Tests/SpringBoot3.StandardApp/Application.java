package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp;

import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentManage;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.Mode;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;

@IntentManage(annotations = Mode.Ignore)
@SpringBootApplication()
@EntityScan(basePackages = {"com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp", "za.myorg.mypackage"})
public class Application {
    public static void main(final String[] args) {
        SpringApplication.run(Application.class, args);
    }
}