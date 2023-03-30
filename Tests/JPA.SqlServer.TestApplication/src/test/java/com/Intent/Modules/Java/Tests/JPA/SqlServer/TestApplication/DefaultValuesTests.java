package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Attributes.DefaultValuesRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Attributes.DefaultValues;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
@AutoConfigureTestDatabase(replace= AutoConfigureTestDatabase.Replace.NONE)
public class DefaultValuesTests {
    @Autowired
    DefaultValuesRepository repository;

    @Test
    void persistenceTest() {
        var val = new DefaultValues();
        repository.save(val);
        var result = repository.findAll();
        assertThat(result).containsOnly(val);
    }
}
