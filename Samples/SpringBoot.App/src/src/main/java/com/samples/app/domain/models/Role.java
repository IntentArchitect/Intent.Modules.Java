package com.samples.app.domain.models;

import javax.validation.constraints.Size;
import javax.validation.constraints.NotNull;
import lombok.Data;
import lombok.EqualsAndHashCode;
import javax.persistence.*;



@EqualsAndHashCode(callSuper = true)
@Entity
@Table(name = "role")
@Data
public class Role extends AbstractEntity {
    private static final long serialVersionUID = 1L;
    
    @Size(max = 50)
    @NotNull
    @Column(name = "name", length = 50, nullable = false)
    private String name;

}
