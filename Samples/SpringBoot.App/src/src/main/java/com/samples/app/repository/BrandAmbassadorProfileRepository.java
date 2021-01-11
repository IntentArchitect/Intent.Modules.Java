package com.samples.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import com.samples.app.domain.models.BrandAmbassadorProfile;
import com.samples.app.intent.IntentMerge;

/**
 * Spring Data JPA repository for the BrandAmbassadorProfile entity.
 */
@IntentMerge
public interface BrandAmbassadorProfileRepository extends JpaRepository<BrandAmbassadorProfile, Long> {
}