package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.data;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface UserRepository extends JpaRepository<User, UUID> {
}
