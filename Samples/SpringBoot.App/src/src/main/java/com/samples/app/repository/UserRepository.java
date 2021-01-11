package com.samples.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.samples.app.domain.models.User;
import com.samples.app.intent.IntentMerge;

/**
 * Spring Data JPA repository for the User entity.
 */
@IntentMerge
public interface UserRepository extends JpaRepository<User, Long> {
}