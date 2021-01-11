package com.samples.app.domain.models;

import javax.validation.constraints.NotNull;
import lombok.Data;
import lombok.EqualsAndHashCode;
import javax.persistence.*;



@EqualsAndHashCode(callSuper = true)
@Entity
@Table(name = "login_session")
@Data
public class LoginSession extends AbstractEntity {
    private static final long serialVersionUID = 1L;
    
    @NotNull
    @Column(name = "timestamp", nullable = false)
    private java.time.LocalDate timestamp;

}
