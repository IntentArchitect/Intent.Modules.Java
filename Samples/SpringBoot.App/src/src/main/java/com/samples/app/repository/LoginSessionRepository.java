package com.samples.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.samples.app.domain.models.LoginSession;
import com.samples.app.intent.IntentMerge;

/**
 * Spring Data JPA repository for the LoginSession entity.
 */
@IntentMerge
public interface LoginSessionRepository extends JpaRepository<LoginSession, Long> {
}