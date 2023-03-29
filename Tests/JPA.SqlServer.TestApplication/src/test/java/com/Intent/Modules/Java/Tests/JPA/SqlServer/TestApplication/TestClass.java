package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations.A_RequiredCompositeRepository;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;

import static org.assertj.core.api.Assertions.*;

@DataJpaTest
@AutoConfigureTestDatabase(replace= AutoConfigureTestDatabase.Replace.NONE)
public class TestClass {
    @Autowired
    A_RequiredCompositeRepository repository;

    @Test
    void findAll() {
        var entities = repository.findAll();
        //assertThat(entities).hasSize(1);
    }
}
