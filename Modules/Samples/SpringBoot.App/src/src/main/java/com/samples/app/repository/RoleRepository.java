package com.samples.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.samples.app.domain.models.Role;
import com.samples.app.intent.IntentMerge;

/**
 * Spring Data JPA repository for the Role entity.
 */
@IntentMerge
public interface RoleRepository extends JpaRepository<Role, Long> {
}