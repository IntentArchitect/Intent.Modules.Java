package com.samples.app.domain.models;

import lombok.Data;
import lombok.EqualsAndHashCode;
import javax.persistence.*;



@EqualsAndHashCode(callSuper = true)
@Entity
@Table(name = "brand_ambassador_profile")
@Data
public class BrandAmbassadorProfile extends AbstractEntity {
    private static final long serialVersionUID = 1L;

}
